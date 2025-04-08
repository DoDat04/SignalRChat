# SignalRChat

Đây là một ứng dụng web chat đơn giản sử dụng **SignalR** – một thư viện của ASP.NET để thêm chức năng real-time (thời gian thực) vào ứng dụng web. Dự án được tạo ra với mục đích học tập và tìm hiểu cách hoạt động cơ bản của SignalR.

## 🚀 Tính năng

- Đăng nhập bằng google
- Gửi và nhận tin nhắn theo thời gian thực
- Giao diện đơn giản, dễ sử dụng
- Hiển thị tên người gửi
- Nhắn tin 1-1 hoặc nhắn tin nhóm

## 🛠️ Công nghệ sử dụng

- ASP.NET Core
- SignalR
- HTML/CSS/JavaScript


## 🔐 Cấu hình đăng nhập Google

Trước tiên, bạn cần tạo một OAuth 2.0 Client ID từ Google Developer Console:

1. Truy cập: https://console.developers.google.com/
2. Tạo Project mới (nếu chưa có)
3. Vào **APIs & Services > Credentials**
4. Tạo **OAuth 2.0 Client ID** với:
   - Ứng dụng web
   - URI chuyển hướng: `https://localhost:5001/signin-google` (tùy theo cấu hình)
5. Sao chép `ClientId` và `ClientSecret`, dán vào `appsettings.json`:

```json
"GoogleKeys": {
  "ClientId": "",
  "ClientSecret": ""
}

