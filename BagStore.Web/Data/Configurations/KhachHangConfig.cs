using BagStore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class KhachHangConfig : IEntityTypeConfiguration<KhachHang>
    {
        public void Configure(EntityTypeBuilder<KhachHang> builder)
        {
            builder.ToTable("KhachHang");

            builder.HasKey(k => k.MaKH);

            builder.Property(k => k.TenKH)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(k => k.SoDienThoai)
                   .HasMaxLength(15);
            builder.HasIndex(k => k.SoDienThoai).IsUnique();

            builder.Property(k => k.DiaChiMacDinh)
                   .HasMaxLength(500);

            builder.Property(k => k.NgayDangKy)
                   .HasDefaultValueSql("GETDATE()");

            // ApplicationUserId
            builder.HasIndex(k => k.ApplicationUserId).IsUnique();

            // Quan hệ FK với ApplicationUser được khai báo trong ApplicationUserConfig
        }
    }
}