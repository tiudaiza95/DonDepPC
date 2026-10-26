using System;
using System.Collections.Generic;
using System.IO;

static class I18n {
 public static string Lang = "vi";

 static readonly Dictionary<string, string> En = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
  // Navigation
  {"Tổng quan", "Dashboard"},
  {"Ứng dụng", "Applications"},
  {"Quét chuyên sâu", "Deep Clean"},
  {"Khởi động", "Startup"},
  {"Công cụ ổ đĩa", "Disk Tools"},
  {"Driver & thiết bị", "Drivers & Devices"},
  {"Nhật ký", "History"},

  // Headings & Subtitles
  {"Tổng quan hệ thống & Giám sát hiệu năng", "System Overview & Performance Monitor"},
  {"Cấu hình máy tính, giám sát tài nguyên thời gian thực và dọn dẹp hệ thống", "Hardware specs, real-time resource monitor and system cleanup"},
  {"Gỡ cài đặt & Quản lý ứng dụng", "Uninstall & Manage Applications"},
  {"Quản lý ứng dụng, gỡ cài đặt và kiểm tra phần còn lại.", "Manage applications, uninstall and verify leftover remnants."},
  {"Quét chuyên sâu toàn diện hệ thống", "Comprehensive Deep System Clean"},
  {"Tích hợp 3 trong 1: Rác & Tàn dư gỡ cài đặt · Tệp dung lượng lớn (>100MB) · Tệp trùng lặp nội dung (SHA-256)", "3-in-1: Leftover remnants · Large files (>100MB) · Duplicate content (SHA-256)"},
  {"Quản lý mục khởi động cùng Windows", "Windows Startup Applications Manager"},
  {"Bật/tắt các ứng dụng chạy ngầm khi khởi động máy để tăng tốc độ khởi động", "Enable/disable background startup items to speed up boot time"},
  {"Chăm sóc & Quản lý ổ đĩa toàn diện", "Disk Care & Partition Management"},
  {"6 bộ công cụ chuyên sâu tối ưu hóa lưu trữ, kiểm tra lỗi và quản lý phân vùng Windows", "6 advanced tools for storage optimization, error checking and partition management"},
  {"Lịch sử gỡ cài đặt & Nhật ký", "Uninstall History & System Activity Logs"},
  {"Driver từ đúng nhà sản xuất.", "Manufacturer Drivers & Devices"},
  {"Kiểm kê tại máy — nguồn hãng — cài gói cục bộ có xác minh", "Local inventory — vendor sources — verified local INF installation"},

  // Dashboard specs
  {"CẤU HÌNH HỆ THỐNG", "SYSTEM SPECIFICATIONS"},
  {"DỌN DẸP HỆ THỐNG & TỆP TẠM", "SYSTEM & TEMP FILE CLEANER"},
  {"Hệ điều hành", "Operating System"},
  {"Bộ vi xử lý (CPU)", "Processor (CPU)"},
  {"Card đồ họa (GPU)", "Graphics (GPU)"},
  {"Bo mạch chủ", "Motherboard"},
  {"Dọn sạch bộ nhớ đệm Google Chrome (Bỏ chọn nếu muốn giữ cache)", "Clean Google Chrome cache (Uncheck to keep cache)"},
  {"Chưa quét. Sẵn sàng quét và tối ưu hệ thống 1-click.", "Ready for 1-click scan and system optimization."},
  {"MỨC DÙNG CPU", "CPU USAGE"},
  {"BỘ NHỚ RAM", "RAM MEMORY"},
  {"Ổ ĐĨA C: (HỆ THỐNG)", "DRIVE C: (SYSTEM)"},
  {"Đang theo dõi", "Monitoring"},
  {"Xung nhịp & tiến trình Windows", "Clock speed & Windows processes"},
  {"Ổ đĩa hệ thống", "System drive"},
  {"trống", "free"},
  {"Tổng", "Total"},
  {"đã dùng", "used"},
  {"Chế độ bảo vệ dữ liệu tích cực: Toàn bộ tác vụ quét dọn hệ thống đều ưu tiên chuyển vào Thùng rác để khôi phục và sao lưu Registry an toàn trước khi xử lý.", "Active Data Protection: All cleanup tasks prioritize Recycle Bin for recovery and backup Registry before processing."},

  // Buttons
  {"⚡ Quét & Tối ưu (1-Click)", "⚡ Scan & Optimize (1-Click)"},
  {"🧹 Dọn mục đã chọn", "🧹 Clean Selected Items"},
  {"🧹 Dọn dẹp Registry", "🧹 Registry Cleaner"},
  {"⚡ Tối ưu RAM", "⚡ Optimize RAM"},
  {"Gỡ mục đã chọn", "Uninstall Selected"},
  {"⚡ Gỡ cưỡng chế", "⚡ Force Uninstall"},
  {"⚡ Gỡ hàng loạt", "⚡ Batch Uninstall"},
  {"Quét phần dư", "Scan Remnants"},
  {"Dọn mục đã chọn", "Clean Selected"},
  {"Phần dư đã ghi nhận", "Recorded Remnants"},
  {"Làm mới", "Refresh"},
  {"Thông tin", "Details"},
  {"Gỡ cài đặt", "Uninstall"},
  {"Bật / Tắt mục khởi động", "Toggle Startup Item"},
  {"Xóa mục khởi động", "Delete Startup Item"},

  // Disk tools
  {"💾 DỌN DUNG LƯỢNG HỆ THỐNG", "💾 SYSTEM DISK CLEANUP"},
  {"Mở Disk Cleanup", "Open Disk Cleanup"},
  {"⚡ TỐI ƯU HÓA HDD VÀ SSD", "⚡ HDD & SSD OPTIMIZATION"},
  {"Mở Tối ưu ổ đĩa", "Open Drive Optimizer"},
  {"🔍 KIỂM TRA LỖI TẬP TIN", "🔍 FILE SYSTEM ERROR CHECK"},
  {"🔍 Quét kiểm tra ổ C:", "🔍 Scan Drive C:"},
  {"Kiểm tra phân vùng Windows", "Windows Partition Check"},
  {"▤ QUẢN LÝ PHÂN VÙNG & LƯU TRỮ", "▤ PARTITION & STORAGE MANAGER"},
  {"Quản lý phân vùng", "Manage Partitions"},
  {"Cài đặt lưu trữ Windows", "Windows Storage Settings"},
  {"▶ PHÂN TÍCH CHIẾM DỤNG Ổ ĐĨA", "▶ DISK SPACE ANALYZER"},
  {"▶ Phân tích thư mục ổ C:", "▶ Analyze Drive C: Folders"},
  {"Xem tệp tạm thời Windows", "View Temporary Files"},
  {"⚡ TỐI ƯU HIBERNATE & COMPACT OS", "⚡ HIBERNATE & COMPACT OS"},
  {"⚡ Tắt Ngủ đông", "⚡ Turn Off Hibernation"},
  {"⚡ Kiểm tra nén Compact OS", "⚡ Check Compact OS State"},

  // Deep Clean
  {"🛡️ Tàn dư gỡ cài đặt", "🛡️ Leftover Remnants"},
  {"📦 Tệp dung lượng lớn", "📦 Large Files"},
  {"📑 Tệp trùng lặp", "📑 Duplicate Files"},
  {"⚡ Quét toàn bộ chuyên sâu (Cả 3 mục)", "⚡ Deep Scan All (3-in-1)"},
  {"Dọn tàn dư đã chọn", "Clean Selected Remnants"},
  {"Dọn tệp lớn đã chọn", "Delete Selected Large Files"},
  {"Tự chọn bản trùng dư thừa", "Auto-select Duplicates"},
  {"Chưa có dữ liệu cho mục này. Hãy bấm '⚡ Quét toàn bộ chuyên sâu (Cả 3 mục)' ở trên để bắt đầu phân tích!", "No data for this category yet. Click '⚡ Deep Scan All (3-in-1)' above to begin scanning!"},

  // Startup
  {"● Đang kích hoạt", "● Active"},
  {"○ Đã tắt", "○ Disabled"},

  // Settings
  {"CÀI ĐẶT & TIỆN ÍCH CHUYÊN SÂU", "ADVANCED SETTINGS & UTILITIES"},
  {"Tùy biến giao diện, ngôn ngữ, an toàn dữ liệu và trung tâm lối tắt hệ thống", "Customize appearance, language, data safety and system shortcuts"},
  {"Giao diện & Ngôn ngữ", "Appearance & Language"},
  {"⚙️ Cài đặt & An toàn", "⚙️ Settings & Safety"},
  {"🛠️ Lối tắt Tiện ích", "🛠️ Utilities & Shortcuts"},
  {"ℹ️ Giới thiệu", "ℹ️ About"},
  {"Chính sách an toàn dữ liệu 100%", "100% Data Safety Policy"},
  {"Trung tâm công cụ nâng cao", "Advanced Tools Center"},
  {"Truy cập nhanh các tính năng dọn dẹp và bảo trì hệ thống 1-chạm:", "Quick 1-click access to system maintenance and cleanup tools:"},
  {"🧹 Dọn dẹp Registry", "🧹 Clean Registry"},
  {"🧩 Tiện ích trình duyệt", "🧩 Browser Extensions"},
  {"⚙️ Dịch vụ bên thứ ba", "⚙️ Third-Party Services"},
  {"📁 Bản sao lưu Registry", "📁 Registry Backups"},
  {"📁 Bản sao lưu Driver", "📁 Driver Backups"},
  {"🗑️ Mở Thùng rác Windows", "🗑️ Open Windows Recycle Bin"},
  {"📝 Mở tệp Nhật ký", "📝 Open Activity Log"},
  {"💾 Công cụ Disk Cleanup", "💾 Disk Cleanup Tool"},
  {"⚡ Tối ưu ổ đĩa Defrag", "⚡ Drive Defrag & Trim Tool"},
  {"☀ Chuyển sang Chế độ sáng", "☀ Switch to Light Mode"},
  {"◐ Chuyển sang Chế độ tối", "◐ Switch to Dark Mode"},
  {"Mở rộng thanh bên", "Expand Sidebar"},
  {"Thu gọn thanh bên", "Collapse Sidebar"}
 };

 public static void Init() {
  try {
   string path = Path.Combine(Engine.Data, "language");
   if (File.Exists(path) && File.ReadAllText(path).Trim().Equals("en", StringComparison.OrdinalIgnoreCase)) {
    Lang = "en";
   } else {
    Lang = "vi";
   }
  } catch { Lang = "vi"; }
 }

 public static void Save(string lang) {
  Lang = lang;
  try {
   Directory.CreateDirectory(Engine.Data);
   File.WriteAllText(Path.Combine(Engine.Data, "language"), lang);
  } catch {}
 }

 public static string T(string text) {
  if (Lang != "en" || string.IsNullOrEmpty(text)) return text;
  string key = text.Trim();
  string val;
  if (En.TryGetValue(key, out val)) return val;
  return text;
 }
}
