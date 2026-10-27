# Dọn Dẹp PC (DonDepPC Pro Max v1.0)

Ứng dụng dọn dẹp hệ thống, tối ưu hiệu năng và gỡ cài đặt chuyên sâu mã nguồn mở dành cho Windows.  
Được thiết kế kết hợp hoàn hảo những ưu điểm vượt trội của **CCleaner** và **IObit Uninstaller**, bổ sung các công cụ bảo trì ổ đĩa và cập nhật driver an toàn tuyệt đối.

![DonDepPC Icon](DonDepPC.png)

---

## 🌟 Tính năng nổi bật trong Phiên bản 1.0 Chính thức

1. **Tổng quan & Giám sát hiệu năng thời gian thực**:
   - Tự động nhận diện cấu hình phần cứng: Hệ điều hành, CPU, GPU, Bo mạch chủ.
   - Giám sát trực quan mức tiêu thụ CPU, RAM và dung lượng ổ C: theo thời gian thực.
   - **Quét & Tối ưu 1-Click**: Tự động giải phóng bộ nhớ đệm RAM, dọn tệp rác hệ thống, xóa cache trình duyệt (Chrome, Edge, Brave, Cốc Cốc, Firefox, Opera, Vivaldi) và báo cáo lỗi CrashDumps/WER.
   - Bộ dọn dẹp Registry mồ côi kèm tính năng tự động sao lưu an toàn UTF-16LE.

2. **Gỡ cài đặt & Quản lý ứng dụng chuyên sâu**:
   - Danh sách phần mềm cài đặt chi tiết kèm dung lượng và ngày cài đặt.
   - **Gỡ cài đặt tiêu chuẩn**: Chạy trình gỡ cài đặt gốc của phần mềm.
   - **Gỡ cưỡng chế (Force Uninstall)**: Dành cho các ứng dụng lỗi, cứng đầu hoặc không còn file uninstaller.
   - **Gỡ hàng loạt (Batch Uninstall)**: Lựa chọn nhiều phần mềm để gỡ tuần tự tiết kiệm thời gian.
   - **Dọn tàn dư sau gỡ**: Quét sạch thư mục mồ côi trong Program Files, AppData và Registry.

3. **Quét chuyên sâu 3-trong-1 (Deep Clean)**:
   - Quét tàn dư gỡ cài đặt và khóa mồ côi.
   - Phân tích tệp dung lượng lớn (>100MB) trên toàn bộ máy tính.
   - Tìm kiếm và đối chiếu tệp trùng lặp nội dung chuẩn mã băm SHA-256.

4. **Chăm sóc & Quản lý ổ đĩa toàn diện**:
   - Tích hợp 6 công cụ mạnh mẽ: Dọn dung lượng Windows, Tối ưu ổ đĩa Defrag/Trim, Kiểm tra lỗi hệ thống tập tin (chkdsk), Quản lý phân vùng, Phân tích chiếm dụng ổ C:, Quản lý tệp ngủ đông (Hibernate) & Nén Compact OS.

5. **Driver từ đúng nhà sản xuất**:
   - Kiểm kê phần cứng máy tính và trạng thái thiết bị.
   - 1-Click mở Device Manager của Windows.
   - Dẫn link tải driver từ đúng trang chủ chính thức của nhà sản xuất, cài đặt gói INF an toàn sau khi đã tự động sao lưu driver.

6. **Đa ngôn ngữ & Giao diện hiện đại**:
   - Hỗ trợ chuyển đổi mượt mà giữa **Tiếng Việt** và **English (US)** trong phần Cài đặt.
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
File thực thi `DonDepPC.exe` sẽ được tạo ngay trong thư mục gốc và tự động chạy bộ kiểm thử `self-test` với 96 kiểm tra tự động đạt chuẩn.
