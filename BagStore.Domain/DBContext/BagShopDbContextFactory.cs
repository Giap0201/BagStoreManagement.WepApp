// Trong thư mục Data (hoặc một thư mục con như Migrations) của dự án BagStore.Data

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO; // Cần thiết cho Path và Directory

using BagStore.Domain.Entities.IdentityModels; // Cho ApplicationUser

// using BagStore.Data.Configurations; // Cho các lớp cấu hình IEntityTypeConfiguration<T>
// ... (các using khác nếu cần) ...

public class BagShopDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Bước 1: Thiết lập đường dẫn cơ sở cho cấu hình.
        // Điều này quan trọng nếu bạn chạy lệnh từ thư mục khác.
        // Ở đây, giả định appsettings.json nằm trong thư mục gốc của BagStore.Data.
        // Nếu appsettings.json nằm ở dự án khác (ví dụ: một Test project),
        // bạn sẽ cần điều chỉnh 'basePath' cho phù hợp.
        var currentDirectory = Directory.GetCurrentDirectory();
        // Nếu bạn chạy lệnh từ thư mục gốc của Solution, và BagStore.Data là một thư mục con:
        // var basePath = Path.Combine(currentDirectory, "BagStore.Data");
        // Hoặc, nếu appsettings.json nằm ngay cạnh file này trong thư mục build output:
        var basePath = currentDirectory;

        // Bước 2: Xây dựng Configuration để đọc appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // optional: false để báo lỗi nếu không tìm thấy
            .Build();

        // Bước 3: Lấy chuỗi kết nối
        // Đảm bảo tên chuỗi kết nối khớp với tên trong appsettings.json của bạn
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Kiểm tra xem chuỗi kết nối có tồn tại không
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json.");
        }

        // Bước 4: Xây dựng DbContextOptions
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        // Bước 5: Trả về một instance của DbContext
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}