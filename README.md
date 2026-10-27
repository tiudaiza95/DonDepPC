# Dọn Dẹp PC (DonDepPC Pro Max v1.1)

Ứng dụng dọn dẹp hệ thống, tối ưu hiệu năng và gỡ cài đặt chuyên sâu mã nguồn mở dành cho Windows.  
Được thiết kế kết hợp hoàn hảo những ưu điểm vượt trội của **CCleaner Professional**, **IObit Uninstaller Pro** và **Revo Uninstaller**, bổ sung các công cụ bảo trì ổ đĩa và cập nhật driver an toàn tuyệt đối.

![DonDepPC Icon](DonDepPC.png)

---

## 🌟 Tính năng nổi bật trong Phiên bản 1.1 (Mới nhất)

1. **Quản lý & Gỡ bỏ Ứng dụng Windows Store (UWP Apps)**:
   - Tự động nhận diện và liệt kê tất cả các ứng dụng cài từ Microsoft Store và ứng dụng hệ thống Windows.
   - Trích xuất biểu tượng chính thức sắc nét từ thư mục gói ứng dụng.
   - Bộ lọc chuyên dụng: Lọc riêng `Ứng dụng Windows Store (UWP)` hoặc `Ứng dụng máy tính (Win32)`.
   - Gỡ bỏ sạch sẽ, an toàn bằng lệnh PowerShell nền không cần can thiệp phức tạp.

2. **Công cụ Giám sát Cài đặt Mới (Setup Monitor / Snapshot Tracker)**:
   - Chụp ảnh snapshot trạng thái hệ thống trước khi cài đặt phần mềm mới (< 0.5s).
   - Tự động đối chiếu sau khi cài đặt để ghi nhận chính xác 100% mọi tệp tin, thư mục và khóa Registry phát sinh.
   - Hỗ trợ **Gỡ bỏ triệt để 100% (Clean Uninstall)**: Xóa sạch toàn bộ tàn dư được ghi trong snapshot.

3. **Quét chuyên sâu 4-trong-1 & Lối tắt hỏng (Deep Clean)**:
   - Bổ sung công cụ phát hiện và dọn dẹp **Lối tắt hỏng (Broken Shortcuts)** trên Desktop và Start Menu.
   - Quét tàn dư gỡ cài đặt và khóa mồ côi.
   - Phân tích tệp dung lượng lớn (>100MB) trên toàn bộ máy tính.
   - Tìm kiếm và đối chiếu tệp trùng lặp nội dung chuẩn mã băm SHA-256.

4. **Biểu đồ Sóng Tài nguyên Thời gian thực (Sparkline Waves)**:
   - Vẽ đường cong sóng tài nguyên chuyển động mượt mà cho **MỨC DÙNG CPU** và **BỘ NHỚ RAM**.
   - Lưu trữ 22 mẫu đo gần nhất với đồ họa vector native siêu nhẹ, phong cách hiện đại.

5. **Tổng quan & Giám sát hiệu năng thời gian thực**:
   - Tự động nhận diện cấu hình phần cứng: Hệ điều hành, CPU, GPU, Bo mạch chủ.
   - **Quét & Tối ưu 1-Click**: Tự động giải phóng bộ nhớ đệm RAM, dọn tệp rác hệ thống, xóa cache trình duyệt (Chrome, Edge, Brave, Cốc Cốc, Firefox, Opera, Vivaldi).
   - Bộ dọn dẹp Registry mồ côi kèm tính năng quét **Phông chữ mồ côi (Orphaned Fonts)** và tự động sao lưu an toàn UTF-16LE.

6. **Chăm sóc & Quản lý ổ đĩa toàn diện**:
   - Tích hợp 6 công cụ mạnh mẽ: Dọn dung lượng Windows, Tối ưu ổ đĩa Defrag/Trim, Kiểm tra lỗi hệ thống tập tin (chkdsk), Quản lý phân vùng, Phân tích chiếm dụng ổ C:, Quản lý tệp ngủ đông (Hibernate) & Nén Compact OS.

7. **Driver từ đúng nhà sản xuất**:
   - Kiểm kê phần cứng máy tính và trạng thái thiết bị.
   - **📄 Xuất báo cáo Driver**: Xuất danh sách toàn bộ driver thiết bị ra tệp TXT trên Desktop chỉ với 1-Click.
   - 1-Click mở Device Manager của Windows.
   - Dẫn link tải driver từ đúng trang chủ chính thức của hãng, cài đặt gói INF an toàn sau khi đã tự động sao lưu.

8. **Đa ngôn ngữ & Giao diện hiện đại**:
   - Hỗ trợ chuyển đổi mượt mà giữa **Tiếng Việt** và **English (US)**.
   - Hỗ trợ Chế độ Sáng / Tối (Dark Mode) và Thu gọn / Mở rộng thanh bên.
   - Lối tắt trực tiếp đến Kho mã nguồn GitHub và Kiểm tra bản cập nhật.

---

## 🛡️ Cam kết an toàn dữ liệu 100%

- Toàn bộ thao tác xóa tệp đều ưu tiên chuyển vào **Thùng rác (Recycle Bin)** của Windows để người dùng có thể khôi phục lại bất kỳ lúc nào.
- Tự động tạo bản sao lưu Registry `.reg` trước khi dọn dẹp.
- Không gửi bất kỳ dữ liệu cá nhân nào lên Internet.

---

## 🛠️ Hướng dẫn Biên dịch từ Mã nguồn

### Yêu cầu môi trường
- Hệ điều hành: Windows 10 / Windows 11 (64-bit)
- .NET Framework 4.8 / .NET SDK

### Lệnh biên dịch
Chạy file PowerShell script:
```powershell
powershell -ExecutionPolicy Bypass -File .\Source\build.ps1
```
Tệp thực thi `DonDepPC.exe` sẽ được tạo ngay tại thư mục gốc.

### Chạy kiểm thử tự động (Self-Test)
```powershell
powershell -ExecutionPolicy Bypass -Command "Start-Process .\DonDepPC.exe -ArgumentList '--self-test' -Wait; Get-Content .\self-test.txt"
```
Kết quả đạt chuẩn: `PASS: 103 checks`.
