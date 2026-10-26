using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;

public class ThirdPartyService {
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public int StartType { get; set; } // 2=Auto, 3=Manual, 4=Disabled
    public string Vendor { get; set; }

    public string StatusLabel {
        get {
            switch (StartType) {
                case 2: return "Tự động (Auto)";
                case 3: return "Thủ công (Manual)";
                case 4: return "Đã tắt (Disabled)";
                default: return "Khác (" + StartType + ")";
            }
        }
    }

    public string CleanPath {
        get {
            if (String.IsNullOrWhiteSpace(ImagePath)) return "";
            string p = ImagePath.Trim();
            if (p.StartsWith("\"")) {
                int next = p.IndexOf('\"', 1);
                if (next > 1) return p.Substring(1, next - 1);
            }
            int sp = p.IndexOf(".exe", StringComparison.OrdinalIgnoreCase);
            if (sp > 0) return p.Substring(0, sp + 4).Trim('\"');
            return p;
        }
    }
}

public static class ServiceTools {
    public static List<ThirdPartyService> GetServices() {
        var list = new List<ThirdPartyService>();
        try {
            using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var servicesKey = baseKey.OpenSubKey(@"SYSTEM\CurrentControlSet\Services")) {
                if (servicesKey == null) return list;
                foreach (string name in servicesKey.GetSubKeyNames()) {
                    try {
                        using (var sk = servicesKey.OpenSubKey(name)) {
                            if (sk == null) continue;
                            string image = sk.GetValue("ImagePath") as string;
                            if (String.IsNullOrWhiteSpace(image)) continue;

                            string lower = image.ToLowerInvariant();
                            // Bỏ qua các service lõi của Windows và svchost mặc định
                            bool isWindowsInternal = lower.Contains(@"system32\svchost.exe") ||
                                                     lower.Contains(@"syswow64\svchost.exe") ||
                                                     (lower.Contains(@"\windows\system32\") && !lower.Contains("driver") && !lower.Contains("nvidia") && !lower.Contains("realtek") && !lower.Contains("amd")) ||
                                                     (lower.Contains(@"\windows\syswow64\") && !lower.Contains("driver") && !lower.Contains("nvidia") && !lower.Contains("realtek") && !lower.Contains("amd"));

                            if (isWindowsInternal) continue;

                            int start = 3;
                            object rawStart = sk.GetValue("Start");
                            if (rawStart is int) start = (int)rawStart;

                            string disp = sk.GetValue("DisplayName") as string;
                            if (String.IsNullOrWhiteSpace(disp)) disp = name;

                            string desc = sk.GetValue("Description") as string ?? "";

                            string vendor = DetectVendor(disp, name, image);

                            list.Add(new ThirdPartyService {
                                Name = name,
                                DisplayName = disp,
                                Description = desc,
                                ImagePath = image,
                                StartType = start,
                                Vendor = vendor
                            });
                        }
                    } catch { }
                }
            }
        } catch { }

        return list.OrderBy(x => x.DisplayName).ToList();
    }

    static string DetectVendor(string disp, string name, string path) {
        string t = (disp + " " + name + " " + path).ToLowerInvariant();
        if (t.Contains("google") || t.Contains("gupdate") || t.Contains("chrome")) return "Google LLC";
        if (t.Contains("adobe")) return "Adobe Inc.";
        if (t.Contains("nvidia")) return "NVIDIA Corporation";
        if (t.Contains("steam") || t.Contains("valve")) return "Valve Corporation";
        if (t.Contains("intel")) return "Intel Corporation";
        if (t.Contains("amd") || t.Contains("radeon")) return "Advanced Micro Devices (AMD)";
        if (t.Contains("realtek")) return "Realtek Semiconductor";
        if (t.Contains("microsoft")) return "Microsoft Corporation";
        if (t.Contains("brave")) return "Brave Software";
        if (t.Contains("coccoc")) return "Cốc Cốc";
        if (t.Contains("vmware")) return "VMware";
        if (t.Contains("virtualbox") || t.Contains("oracle")) return "Oracle";
        if (t.Contains("anydesk")) return "AnyDesk Software";
        if (t.Contains("teamviewer")) return "TeamViewer";
        if (t.Contains("epic") || t.Contains("unreal")) return "Epic Games";
        if (t.Contains("riot")) return "Riot Games";
        if (t.Contains("blizzard") || t.Contains("battle.net")) return "Blizzard Entertainment";
        if (t.Contains("asus")) return "ASUSTeK Computer";
        if (t.Contains("gigabyte")) return "Giga-Byte Technology";
        if (t.Contains("msi") || t.Contains("micro-star")) return "Micro-Star International";
        return "Bên thứ ba khác";
    }

    public static bool SetStartType(string serviceName, int newStartType, out string message) {
        message = "";
        string arg = "";
        switch (newStartType) {
            case 2: arg = "start= auto"; break;
            case 3: arg = "start= demand"; break;
            case 4: arg = "start= disabled"; break;
            default:
                message = "Kiểu khởi động không hợp lệ.";
                return false;
        }

        try {
            var psi = new ProcessStartInfo("sc.exe", "config \"" + serviceName + "\" " + arg) {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using (var proc = Process.Start(psi)) {
                if (proc == null) {
                    message = "Không thể khởi động sc.exe.";
                    return false;
                }
                string stdout = proc.StandardOutput.ReadToEnd();
                string stderr = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                if (proc.ExitCode == 0) {
                    message = "Cập nhật thành công!";
                    return true;
                }

                // Nếu sc.exe thất bại do quyền (Access Denied = 5)
                if (proc.ExitCode == 5 || stderr.ToLowerInvariant().Contains("access is denied") || stdout.ToLowerInvariant().Contains("access is denied")) {
                    message = "Cần quyền Quản trị viên (Run as administrator) để thay đổi thiết lập dịch vụ này.";
                    return false;
                }

                message = !String.IsNullOrWhiteSpace(stderr) ? stderr.Trim() : stdout.Trim();
                return false;
            }
        } catch (Exception ex) {
            message = ex.Message;
            return false;
        }
    }
}
