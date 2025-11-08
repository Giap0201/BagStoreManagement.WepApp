🛍️ BagStoreManagement.WebApp — Hệ thống Quản lý Cửa hàng Túi Xách

Dự án website thương mại điện tử (E-commerce) được xây dựng bằng ASP.NET Core MVC với kiến trúc Repository Pattern và Service Layer.
Hệ thống được thiết kế cho hai đối tượng người dùng chính:

👩‍💼 Admin: Quản trị, quản lý sản phẩm, đơn hàng, khách hàng

👗 Client: Khách hàng mua sắm trực tuyến

💻 Công nghệ Sử dụng
Thành phần	Công nghệ
Backend	ASP.NET Core (MVC & Web API)
Database	Entity Framework Core (Code-First)
Database Server	SQL Server
Kiến trúc	Repository Pattern, Service Layer, Areas
Frontend (Client)	Razor Pages, JavaScript, jQuery, AJAX, Bootstrap
Frontend (Admin)	Razor Pages, Bootstrap, Chart.js (thống kê biểu đồ)
📁 Cấu trúc Dự án

Cấu trúc project được tổ chức theo mô hình phân lớp rõ ràng để dễ bảo trì và mở rộng:

BagStoreManagement.WebApp/
│
├── Areas/
│   ├── Admin/         # Khu vực quản trị (Controllers, Views, Models)
│   └── Client/        # Khu vực khách hàng (Controllers, Views, Models)
│
├── Controllers/
│   └── Api/           # Các API endpoint (JSON) cho AJAX (giỏ hàng, lọc, v.v.)
│
├── Data/
│   ├── BagStoreDbContext.cs
│   └── Configurations/ # File cấu hình cho EF Core
│
├── Migrations/         # Các file migration (EF Core)
│
├── Models/
│   ├── Entities/       # Lớp ánh xạ bảng DB
│   ├── DTOs/           # Data Transfer Objects (truyền dữ liệu API)
│   └── ViewModels/     # Truyền dữ liệu từ Controller → View
│
├── Repositories/
│   ├── Interfaces/     # Interface định nghĩa repository
│   └── Implementations/# Lớp triển khai repository
│
├── Services/
│   ├── Interfaces/     # Interface của service
│   └── Implementations/# Triển khai logic nghiệp vụ
│
├── wwwroot/
│   ├── LayoutAdmin/    # CSS, JS, hình ảnh cho theme Admin
│   ├── LayoutClient/   # CSS, JS, hình ảnh cho theme Client
│   └── uploads/        # Hình ảnh sản phẩm người dùng tải lên
│
└── appsettings.json

🌟 Danh mục Chức năng
🧩 1. Phân vùng Quản trị (Admin)

Dashboard: Thống kê doanh thu, đơn hàng, khách hàng, sản phẩm (Chart.js)

Quản lý Sản phẩm (CRUD): Thêm, sửa, xóa sản phẩm

Quản lý Chi tiết Sản phẩm: Màu sắc, kích thước, tồn kho, hình ảnh

Quản lý Thuộc tính: Danh mục thương hiệu, chất liệu, màu sắc, kích thước, loại túi

Quản lý Đơn hàng: Xem danh sách, chi tiết, cập nhật trạng thái

Quản lý Khách hàng: Danh sách khách hàng đã đăng ký

🛒 2. Phân vùng Khách hàng (Client)

Trang chủ: Hiển thị sản phẩm mới, nổi bật

Cửa hàng (Shop):

Xem tất cả sản phẩm (có phân trang)

Bộ lọc (theo giá, thương hiệu, loại túi, màu sắc, kích thước)

Tìm kiếm sản phẩm theo tên

Chi tiết sản phẩm: Hình ảnh, thông tin, tùy chọn màu/size

Giỏ hàng: Thêm, xóa, cập nhật (AJAX, API)

Thanh toán (Checkout): Nhập thông tin giao hàng, đặt hàng

Tài khoản người dùng: Đăng ký, đăng nhập, cập nhật thông tin cá nhân

Lịch sử đơn hàng: Xem lại đơn hàng đã đặt

🚀 Hướng dẫn Cài đặt & Khởi chạy
1️⃣ Yêu cầu Hệ thống

.NET 6 SDK (hoặc cao hơn)

Visual Studio 2022

SQL Server 2019 (hoặc LocalDB)

2️⃣ Cấu hình Database

Mở file appsettings.json và cập nhật chuỗi kết nối:

"ConnectionStrings": {
  "DefaultConnection": "Server=TEN_SERVER_CUA_BAN;Database=BagStoreDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}


Mở Package Manager Console trong Visual Studio và chạy lệnh sau:

Update-Database


→ Lệnh này sẽ tạo toàn bộ cơ sở dữ liệu theo các Migration có sẵn.

3️⃣ Khởi chạy Ứng dụng

Mở file solution BagStoreManagement.WebApp.sln bằng Visual Studio.

Chọn BagStore.Web làm Startup Project.

Nhấn F5 hoặc nút ▶ Run để chạy ứng dụng.

🔗 Truy cập Ứng dụng

Trang Khách hàng (Client):
👉 https://localhost:<PORT>/

Trang Quản trị (Admin):
👉 https://localhost:<PORT>/Admin

📜 Giấy phép & Bản quyền

Dự án được phát triển phục vụ mục đích học tập và nghiên cứu.
Tác giả giữ toàn quyền nội dung và cấu trúc mã nguồn.
Vui lòng ghi nguồn khi sử dụng hoặc chỉnh sửa.

🧠 Tác giả & Liên hệ

Tác giả: Nguyễn Hữu Giáp, Nguyễn Khánh Sơn, Nguyễn Đăng Trung, Lê Xuân Thành, Phạm Duy Nghĩa

Liên hệ: (cập nhật nếu cần)

Phiên bản: v1.0.0

Ngày cập nhật: 2025