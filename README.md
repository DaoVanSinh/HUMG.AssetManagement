<div align="center">
  <img src="https://raw.githubusercontent.com/DaoVanSinh/HUMG.AssetManagement/main/docs/images/humg_logo.png" alt="HUMG Logo" width="200"/>
  
  # 🏢 HỆ THỐNG QUẢN LÝ TÀI SẢN HUMG
  
  ### Trường Đại học Mỏ - Địa chất Hà Nội
  ### Hanoi University of Mining and Geology
  
  [![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
  [![MAUI](https://img.shields.io/badge/MAUI-Multi--platform-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/apps/maui)
  [![SQLite](https://img.shields.io/badge/SQLite-3.0-003B57?style=flat&logo=sqlite)](https://www.sqlite.org/)
  [![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
  
  **Bài tập lớn môn Lập trình .NET - Học kỳ IB năm 2025**
  
  [Tính năng](#-tính-năng-chính) • [Demo](#-demo) • [Cài đặt](#-cài-đặt) • [Liên hệ](#-liên-hệ)
  
</div>

---

## 📖 Giới thiệu

**Hệ thống Quản lý Tài sản HUMG** là ứng dụng di động đa nền tảng được phát triển bằng **.NET MAUI**, giúp quản lý hiệu quả tài sản của Trường Đại học Mỏ - Địa chất Hà Nội.

### 🎯 Mục tiêu

- ✅ Số hóa quy trình quản lý tài sản
- ✅ Theo dõi trạng thái thiết bị real-time
- ✅ Báo cáo và thống kê tự động
- ✅ Hỗ trợ Windows và Android

---

## ✨ Tính năng chính

### 🔐 Quản lý Người dùng
- 🔑 Đăng nhập/Đăng ký tài khoản
- 🔒 Phân quyền Admin/Staff
- 🔄 Quên mật khẩu và đặt lại
- ⚙️ Đổi mật khẩu với đo độ mạnh

### 📦 Quản lý Tài sản
- ➕ Thêm tài sản với ảnh minh họa
- ✏️ Cập nhật thông tin, trạng thái
- 🗑️ Xóa tài sản (có xác nhận)
- 🔍 Tìm kiếm theo tên, mã, vị trí
- 🎯 Lọc theo trạng thái
- 📄 Xem chi tiết đầy đủ

### 📊 Báo cáo & Thống kê
- 📈 Thống kê tổng quan
- 📉 Phân tích theo trạng thái
- 📊 Phân loại theo loại tài sản
- 🏢 Thống kê theo phòng ban

### 📅 Lịch sử Thay đổi
- 📝 Tracking mọi thao tác
- 👤 Ghi nhận người thực hiện
- 🕐 Timestamp chính xác
- 🔍 Lọc và tìm kiếm

---

## 🛠️ Công nghệ

<div align="center">

| Công nghệ | Phiên bản |
|-----------|-----------|
| .NET | 9.0 |
| .NET MAUI | 9.0 |
| Entity Framework Core | 9.0.0 |
| SQLite | 3.x |
| C# | 12.0 |

</div>

---

## 📸 Demo

### 🔐 Đăng nhập

![Login](https://raw.githubusercontent.com/DaoVanSinh/HUMG.AssetManagement/main/docs/images/screenshots/login.png)

*Giao diện đăng nhập với logo HUMG*

---

### 🏠 Trang chủ

![Main Page](https://raw.githubusercontent.com/DaoVanSinh/HUMG.AssetManagement/main/docs/images/screenshots/main.png)

*Dashboard với thống kê tổng quan*

---

### 📋 Danh sách Tài sản

![Asset List](https://raw.githubusercontent.com/DaoVanSinh/HUMG.AssetManagement/main/docs/images/screenshots/asset-list.png)

*Danh sách tài sản với tìm kiếm và lọc*

---

### ➕ Thêm/Sửa Tài sản

![Add Edit](https://raw.githubusercontent.com/DaoVanSinh/HUMG.AssetManagement/main/docs/images/screenshots/add-edit.png)

*Form nhập liệu với validation*

---

### 🔍 Chi tiết Tài sản

![Detail](https://raw.githubusercontent.com/DaoVanSinh/HUMG.AssetManagement/main/docs/images/screenshots/asset-detail.png)

*Xem thông tin chi tiết và lịch sử*

---

### 📊 Báo cáo Thống kê

![Report](https://raw.githubusercontent.com/DaoVanSinh/HUMG.AssetManagement/main/docs/images/screenshots/report.png)

*Báo cáo với biểu đồ trực quan*

---

### 📅 Lịch sử Thay đổi

![History](https://raw.githubusercontent.com/DaoVanSinh/HUMG.AssetManagement/main/docs/images/screenshots/history.png)

*Tracking toàn bộ hoạt động*

---

## 💻 Yêu cầu Hệ thống

### 🪟 Windows
- Windows 10 (1809+) hoặc Windows 11
- Visual Studio 2022 (v17.14+)
- .NET 9.0 SDK
- 4GB RAM (khuyến nghị 8GB)

### 🤖 Android
- Android 5.0 (API 21) trở lên
- 2GB RAM
- 100MB dung lượng trống

---

## 🚀 Cài đặt

### 1️⃣ Clone Repository
```bash
git clone https://github.com/DaoVanSinh/HUMG.AssetManagement.git
cd HUMG.AssetManagement
```

### 2️⃣ Mở Solution
```bash
start HUMG.AssetManagement.sln
```

Hoặc: **Visual Studio** → **File** → **Open** → **Project/Solution**

### 3️⃣ Restore Packages
```bash
dotnet restore
```

### 4️⃣ Build
```bash
dotnet build
```

### 5️⃣ Chạy

#### Windows
```bash
dotnet run --framework net9.0-windows
```

#### Android
- Chọn **Android Emulator**
- Nhấn **F5**

---

## 📖 Hướng dẫn Sử dụng

### Đăng nhập lần đầu
```
Username: admin
Password: admin123
```

### Quy trình cơ bản

1. **Đăng nhập** → Trang chủ
2. **Danh sách tài sản** → Xem tất cả
3. Click **+** → Thêm tài sản mới
4. Click vào tài sản → Xem chi tiết
5. **Báo cáo** → Xem thống kê
6. **Lịch sử** → Theo dõi thay đổi

---

## 📁 Cấu trúc Project
```
HUMG.AssetManagement/
├── 📂 Models/              # Data models
├── 📂 Views/               # XAML pages
├── 📂 ViewModels/          # MVVM logic
├── 📂 Data/                # Database context
├── 📂 Services/            # Business logic
├── 📂 Resources/           # Images, fonts, styles
├── 📂 Platforms/           # Platform-specific code
│   ├── Android/
│   └── Windows/
└── 📄 README.md
```

---

## 💾 Database Schema
```
┌─────────────┐      ┌─────────────────┐
│   Users     │      │    Assets       │
├─────────────┤      ├─────────────────┤
│ Id (PK)     │◄─────│ CreatedBy (FK)  │
│ Username    │ 1:N  │ Id (PK)         │
│ Password    │      │ AssetCode       │
│ FullName    │      │ AssetName       │
│ Email       │      │ Status          │
└─────────────┘      └─────────────────┘
       │                      │
       └──────────┬───────────┘
                  │ 1:N
           ┌──────▼─────────┐
           │ AssetHistories │
           ├────────────────┤
           │ Id (PK)        │
           │ AssetId (FK)   │
           │ Action         │
           │ ActionBy (FK)  │
           └────────────────┘
```

---

## 🐛 Troubleshooting

<details>
<summary><b>Lỗi "Android SDK not found"</b></summary>

**Giải pháp:**
1. Tools → Android → Android SDK Manager
2. Cài Android SDK Platform 34
3. Restart Visual Studio
</details>

<details>
<summary><b>Database không hoạt động</b></summary>

**Giải pháp:**
1. Xóa file database cũ
2. Clean + Rebuild
3. Chạy lại app
</details>

---

## 👥 Tác giả

<div align="center">

### 🎓 Sinh viên

**Đào Văn Sinh**

📧 [Sinh2004tt@gmail.com](mailto:Sinh2004tt@gmail.com)  
🏫 Lớp: DCCTCT67_05A  
🆔 MSSV: 2221050214

[![GitHub](https://img.shields.io/badge/GitHub-DaoVanSinh-181717?style=for-the-badge&logo=github)](https://github.com/DaoVanSinh)

---

### 👨‍🏫 Giảng viên hướng dẫn

**ThS. Đặng Hữu Nghị**

Khoa Công nghệ Thông tin  
Trường Đại học Mỏ - Địa chất Hà Nội

</div>

---

## 📅 Thông tin Dự án

- **Bắt đầu:** Tháng 11/2024
- **Hoàn thành:** Tháng 12/2024
- **Môn học:** Lập trình .NET + BTL
- **Học kỳ:** IB - Năm 2025

---

## 📄 License

MIT License - Xem file [LICENSE](LICENSE) để biết thêm chi tiết.

---

## 🙏 Lời Cảm ơn

- **ThS. Đặng Hữu Nghị** - Giảng viên hướng dẫn
- **Khoa CNTT** - HUMG
- **Microsoft** - .NET MAUI Framework
- **Cộng đồng .NET Vietnam**

---

## 📞 Liên hệ

- 📧 Email: [Sinh2004tt@gmail.com](mailto:Sinh2004tt@gmail.com)
- 🐙 GitHub: [@DaoVanSinh](https://github.com/DaoVanSinh)
- 🏫 Trường: Đại học Mỏ - Địa chất Hà Nội

---

<div align="center">

### ⭐ Nếu thấy hữu ích, hãy cho repo một Star! ⭐

**Made with ❤️ by Đào Văn Sinh**

**© 2025 Hanoi University of Mining and Geology**

</div>
```





