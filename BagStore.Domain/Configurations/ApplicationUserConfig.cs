// Trong thư mục Data/Configurations/ApplicationUserConfig.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BagStore.Domain.Entities.IdentityModels; // Đảm bảo đúng namespace cho ApplicationUser

public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Cấu hình các thuộc tính tùy chỉnh của ApplicationUser
        builder.Property(u => u.HoTen).HasMaxLength(250);
        builder.Property(u => u.NgayTaoTaiKhoan).HasDefaultValueSql("GETDATE()");

        // vì các mối quan hệ này đã được chuyển sang KhachHangProfile và NhanVienProfile.
    }
}