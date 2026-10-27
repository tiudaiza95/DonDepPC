using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Win32;
using Microsoft.VisualBasic.FileIO;

public class SnapshotSession {
    public DateTime StartTime { get; set; }
    public Dictionary<string, DateTime> ExistingFolders { get; set; }
    public HashSet<string> UninstallKeysHklm { get; set; }
    public HashSet<string> UninstallKeysHkcu { get; set; }
    public HashSet<string> RunValuesHklm { get; set; }
    public HashSet<string> RunValuesHkcu { get; set; }

    public SnapshotSession() {
        ExistingFolders = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        UninstallKeysHklm = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        UninstallKeysHkcu = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        RunValuesHklm = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        RunValuesHkcu = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}

public class SnapshotRecord {
    public string AppName { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> CreatedFolders { get; set; }
    public List<string> CreatedFiles { get; set; }
    public List<string> CreatedRegistryKeys { get; set; }
    public List<string> CreatedRegistryValues { get; set; }

    public SnapshotRecord() {
        CreatedFolders = new List<string>();
        CreatedFiles = new List<string>();
        CreatedRegistryKeys = new List<string>();
        CreatedRegistryValues = new List<string>();
    }
}

public static class SetupMonitor {
    static readonly string[] StandardRoots = {
        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs"),
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
    };

    public static string SnapshotDir {
        get {
            string dir = Path.Combine(Engine.Data, "Snapshots");
            Directory.CreateDirectory(dir);
            return dir;
        }
    }

    public static SnapshotSession StartSession() {
        var s = new SnapshotSession { StartTime = DateTime.UtcNow };

        // 1. Chụp ảnh thư mục
        foreach (var root in StandardRoots.Where(r => !string.IsNullOrEmpty(r) && Directory.Exists(r))) {
            try {
                foreach (var d in Directory.EnumerateDirectories(root)) {
                    try {
                        s.ExistingFolders[d] = Directory.GetLastWriteTimeUtc(d);
                    } catch {}
                }
            } catch {}
        }

        // 2. Chụp ảnh Registry Uninstall & Run
        Action<RegistryHive, HashSet<string>, HashSet<string>> snapReg = (hive, unKeys, runVals) => {
            try {
                using (var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64)) {
                    using (var un = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall")) {
                        if (un != null) foreach (var k in un.GetSubKeyNames()) unKeys.Add(k);
                    }
                    using (var run = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run")) {
                        if (run != null) foreach (var v in run.GetValueNames()) runVals.Add(v);
                    }
                }
            } catch {}
        };

        snapReg(RegistryHive.LocalMachine, s.UninstallKeysHklm, s.RunValuesHklm);
        snapReg(RegistryHive.CurrentUser, s.UninstallKeysHkcu, s.RunValuesHkcu);

        return s;
    }

    public static SnapshotRecord CompleteSession(SnapshotSession session, string appName) {
        var record = new SnapshotRecord {
            AppName = string.IsNullOrWhiteSpace(appName) ? "Phần mềm chưa đặt tên" : appName.Trim(),
            CreatedAt = DateTime.Now
        };

        DateTime threshold = session.StartTime.AddSeconds(-2);

        // 1. Đối chiếu Thư mục & Tệp phát sinh
        foreach (var root in StandardRoots.Where(r => !string.IsNullOrEmpty(r) && Directory.Exists(r))) {
            try {
                foreach (var d in Directory.EnumerateDirectories(root)) {
                    DateTime oldTime;
                    bool isNew = !session.ExistingFolders.TryGetValue(d, out oldTime);
                    bool wasModified = isNew || (Directory.GetLastWriteTimeUtc(d) > threshold);

                    if (isNew) {
                        record.CreatedFolders.Add(d);
                        // Duyệt tệp bên trong thư mục mới
                        try {
                            foreach (var f in Directory.EnumerateFiles(d, "*", System.IO.SearchOption.AllDirectories)) {
                                record.CreatedFiles.Add(f);
                            }
                        } catch {}
                    } else if (wasModified) {
                        // Thư mục cũ nhưng có thay đổi: tìm các tệp mới tạo sau threshold
                        try {
                            foreach (var f in Directory.EnumerateFiles(d, "*", System.IO.SearchOption.TopDirectoryOnly)) {
                                if (File.GetCreationTimeUtc(f) > threshold || File.GetLastWriteTimeUtc(f) > threshold) {
                                    record.CreatedFiles.Add(f);
                                }
                            }
                        } catch {}
                    }
                }
            } catch {}
        }

        // 2. Đối chiếu Registry
        Action<RegistryHive, HashSet<string>, HashSet<string>, string> diffReg = (hive, oldUn, oldRun, hiveName) => {
            try {
                using (var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64)) {
                    using (var un = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall")) {
                        if (un != null) {
                            foreach (var k in un.GetSubKeyNames()) {
                                if (!oldUn.Contains(k)) {
                                    record.CreatedRegistryKeys.Add(hiveName + @"\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + k);
                                }
                            }
                        }
                    }
                    using (var run = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run")) {
                        if (run != null) {
                            foreach (var v in run.GetValueNames()) {
                                if (!oldRun.Contains(v)) {
                                    record.CreatedRegistryValues.Add(hiveName + @"\SOFTWARE\Microsoft\Windows\CurrentVersion\Run [" + v + "]");
                                }
                            }
                        }
                    }
                }
            } catch {}
        };

        diffReg(RegistryHive.LocalMachine, session.UninstallKeysHklm, session.RunValuesHklm, "HKLM");
        diffReg(RegistryHive.CurrentUser, session.UninstallKeysHkcu, session.RunValuesHkcu, "HKCU");

        // Lưu vào file XML
        SaveRecord(record);
        return record;
    }

    public static void SaveRecord(SnapshotRecord record) {
        try {
            string safeName = Engine.Norm(record.AppName);
            if (string.IsNullOrEmpty(safeName)) safeName = Guid.NewGuid().ToString("N");
            string path = Path.Combine(SnapshotDir, safeName + ".xml");
            using (var f = File.Create(path)) {
                new XmlSerializer(typeof(SnapshotRecord)).Serialize(f, record);
            }
        } catch {}
    }

    public static List<SnapshotRecord> LoadAllRecords() {
        var list = new List<SnapshotRecord>();
        try {
            foreach (var f in Directory.GetFiles(SnapshotDir, "*.xml")) {
                try {
                    using (var fs = File.OpenRead(f)) {
                        var rec = (SnapshotRecord)new XmlSerializer(typeof(SnapshotRecord)).Deserialize(fs);
                        if (rec != null) list.Add(rec);
                    }
                } catch {}
            }
        } catch {}
        return list.OrderByDescending(r => r.CreatedAt).ToList();
    }

    public static void DeleteRecord(SnapshotRecord record) {
        if (record == null) return;
        try {
            string safeName = Engine.Norm(record.AppName);
            string path = Path.Combine(SnapshotDir, safeName + ".xml");
            if (File.Exists(path)) File.Delete(path);
        } catch {}
    }

    public static void UninstallMonitoredApp(SnapshotRecord record, out int filesDeleted, out int regDeleted, out List<string> errors) {
        filesDeleted = 0;
        regDeleted = 0;
        errors = new List<string>();

        // 1. Xóa tệp ghi nhận (Gửi vào Recycle Bin để bảo đảm an toàn)
        foreach (var f in record.CreatedFiles) {
            try {
                if (File.Exists(f)) {
                    FileSystem.DeleteFile(f, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    filesDeleted++;
                }
            } catch (Exception ex) {
                errors.Add("Tệp " + f + ": " + ex.Message);
            }
        }

        // 2. Xóa thư mục mới tạo (Nếu trống hoặc chỉ chứa tệp của nó)
        foreach (var d in record.CreatedFolders.OrderByDescending(x => x.Length)) {
            try {
                if (Directory.Exists(d)) {
                    FileSystem.DeleteDirectory(d, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
            } catch (Exception ex) {
                errors.Add("Thư mục " + d + ": " + ex.Message);
            }
        }

        // 3. Xóa Registry Keys
        foreach (var k in record.CreatedRegistryKeys) {
            try {
                bool isHklm = k.StartsWith("HKLM\\", StringComparison.OrdinalIgnoreCase);
                var hive = isHklm ? RegistryHive.LocalMachine : RegistryHive.CurrentUser;
                string subPath = k.Substring(5);
                int lastSlash = subPath.LastIndexOf('\\');
                if (lastSlash > 0) {
                    string parent = subPath.Substring(0, lastSlash);
                    string child = subPath.Substring(lastSlash + 1);
                    using (var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                    using (var pk = baseKey.OpenSubKey(parent, true)) {
                        if (pk != null) {
                            pk.DeleteSubKeyTree(child, false);
                            regDeleted++;
                        }
                    }
                }
            } catch (Exception ex) {
                errors.Add("Registry " + k + ": " + ex.Message);
            }
        }

        // 4. Xóa Registry Values
        foreach (var v in record.CreatedRegistryValues) {
            try {
                int bracket = v.IndexOf('[');
                if (bracket > 0 && v.EndsWith("]")) {
                    string fullKey = v.Substring(0, bracket).Trim();
                    string valName = v.Substring(bracket + 1, v.Length - bracket - 2).Trim();
                    bool isHklm = fullKey.StartsWith("HKLM\\", StringComparison.OrdinalIgnoreCase);
                    var hive = isHklm ? RegistryHive.LocalMachine : RegistryHive.CurrentUser;
                    string subPath = fullKey.Substring(5);
                    using (var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                    using (var sk = baseKey.OpenSubKey(subPath, true)) {
                        if (sk != null) {
                            sk.DeleteValue(valName, false);
                            regDeleted++;
                        }
                    }
                }
            } catch (Exception ex) {
                errors.Add("Registry Value " + v + ": " + ex.Message);
            }
        }

        // Xóa bản ghi sau khi hoàn tất
        DeleteRecord(record);
    }
}
