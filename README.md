🛍️ BagStoreManagement.WebApp: Hệ thống Quản lý Cửa hàng Túi Xách
Đây là dự án website thương mại điện tử (E-commerce) được xây dựng bằng ASP.NET Core MVC với kiến trúc Repository Pattern và Service Layer. Hệ thống được thiết kế để phục vụ hai đối tượng người dùng chính: Khách hàng (Client) mua sắm và Người quản trị (Admin) vận hành hệ thống.
💻 Công nghệ Sử dụng
Backend: ASP.NET Core (MVC & Web API)
Database: Entity Framework Core (Code-First)
Database Server: SQL Server
Kiến trúc: Repository Pattern, Service Layer, Areas
Frontend (Client): Razor Pages, JavaScript, jQuery, AJAX, Bootstrap
Frontend (Admin): Razor Pages, Bootstrap, Chart.js (cho biểu đồ thống kê)
📁 Cấu trúc Project
Dự án được tổ chức theo kiến trúc phân lớp rõ ràng để dễ dàng bảo trì và mở rộng:
/Areas: Phân vùng chức năng chính.
	/Admin: Chứa Controllers, Views, và Models cho trang quản trị.
	/Client: Chứa Controllers, Views, và Models cho trang khách hàng (shop).
/Controllers/Api: Chứa các API endpoints (trả về JSON) để xử lý các tác vụ bất đồng bộ (AJAX) như giỏ hàng, lọc sản phẩm, và các thao tác admin.
/Data: Chứa BagStoreDbContext và các file cấu hình (Configurations) của Entity Framework Core.
/Migrations: Chứa các file migration đã được tạo bởi EF Core.
/Models:
	/Entities: Các lớp đại diện cho cấu trúc bảng trong database.
	/DTOs: (Data Transfer Objects) Các đối tượng dùng để truyền dữ liệu giữa các lớp và API.
	/ViewModels: Các đối tượng dùng để truyền dữ liệu từ Controller sang View.
/Repositories: Lớp chịu trách nhiệm truy cập dữ liệu (CRUD) trực tiếp với DbContext.
	/Interfaces: Các Interface của Repository.
	/Implementations: Các lớp triển khai (Implement) logic của Interface.
/Services: Lớp chịu trách nhiệm xử lý logic nghiệp vụ (business logic), gọi đến Repositories để lấy dữ liệu.
	/Interfaces: Các Interface của Service.
	/Implementations: Các lớp triển khai logic nghiệp vụ.
/wwwroot: Chứa tài nguyên tĩnh (static files).
/LayoutAdmin: CSS, JS, hình ảnh cho theme Admin.
/LayoutClient: CSS, JS, hình ảnh cho theme Client (Shop).
/uploads: Thư mục lưu trữ hình ảnh sản phẩm do người dùng tải lên.
🌟 Danh mục Chức năng
1. Phân vùng Quản trị (Admin)
Dashboard: Thống kê tổng quan doanh thu, đơn hàng, khách hàng và sản phẩm (sử dụng Chart.js).
Quản lý Sản phẩm (CRUD): Thêm, sửa, xóa sản phẩm.
Quản lý Chi tiết Sản phẩm: Quản lý các biến thể (màu sắc, kích thước), tồn kho và hình ảnh.
Quản lý Thuộc tính: Quản lý các danh mục (Thương hiệu, Chất liệu, Màu sắc, Kích thước, Loại túi).
Quản lý Đơn hàng: Xem danh sách, xem chi tiết và cập nhật trạng thái đơn hàng.
Quản lý Khách hàng: Xem danh sách khách hàng đã đăng ký.
2. Phân vùng Khách hàng (Client)
Trang chủ: Hiển thị sản phẩm mới, sản phẩm nổi bật.
Trang Cửa hàng (Shop):
Xem tất cả sản phẩm (có phân trang).
Bộ lọc sản phẩm (theo giá, danh mục, thương hiệu, màu sắc, kích thước).
Tìm kiếm: Tìm kiếm sản phẩm theo tên.
Chi tiết Sản phẩm: Xem thông tin, hình ảnh và các tùy chọn (màu, size) của sản phẩm.
Giỏ hàng (Sử dụng API/AJAX): Thêm, xóa, cập nhật số lượng sản phẩm.
Thanh toán (Checkout): Nhập thông tin giao hàng và đặt hàng.
Quản lý Tài khoản: Đăng ký, đăng nhập, đăng xuất, xem/cập nhật thông tin cá nhân.
Lịch sử Đơn hàng: Cho phép người dùng đã đăng nhập xem lại các đơn hàng đã đặt
🚀 Hướng dẫn Cài đặt & Khởi chạy
1. Yêu cầu
.NET 6 SDK (hoặc phiên bản mới hơn)
Visual Studio 2022
SQL Server 2019 (hoặc LocalDB)
2. Cấu hình Database
Mở file BagStore.Web/appsettings.json.
Tìm đến phần "ConnectionStrings" và thay đổi giá trị của "DefaultConnection" để trỏ đến SQL Server của bạn.
JSON
"ConnectionStrings": {
  "DefaultConnection": "Server=TEN_SERVER_CUA_BAN;Database=BagStoreDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
Mở Package Manager Console trong Visual Studio.
Chạy lệnh sau để áp dụng các Migrations và tạo cơ sở dữ liệu:
PowerShell
Update-Database
4. Khởi chạy Ứng dụng
Mở file BagStoreManagement.WepApp.sln bằng Visual Studio 2022.
Đảm bảo BagStore.Web được chọn làm dự án khởi động (Startup Project).
Nhấn F5 hoặc nút Run (▶) để bắt đầu chạy dự án.
