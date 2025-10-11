using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class DanhGiaConfig : IEntityTypeConfiguration<DanhGia>
    {
        public void Configure(EntityTypeBuilder<DanhGia> builder)
        {
            builder.ToTable("DanhGia");

            builder.HasKey(x => x.MaDanhGia);

            builder.Property(x => x.Diem)
                   .IsRequired();
            builder.HasCheckConstraint("CK_DanhGia_Diem", "[Diem] >= 1 AND [Diem] <= 5");

            builder.Property(x => x.NoiDung)
                   .HasMaxLength(500);

            builder.Property(x => x.NgayDanhGia)
                   .HasDefaultValueSql("GETDATE()");

            // Unique constraint tránh đánh giá trùng
            // Unique constraint: mỗi khách chỉ đánh giá mỗi sản phẩm 1 lần
            builder.HasIndex(x => new { x.MaKH, x.MaSP }).IsUnique();

            // Quan hệ với KhachHang
            builder.HasOne(x => x.KhachHang)
                   .WithMany(k => k.DanhGias)
                   .HasForeignKey(x => x.MaKH)
                   .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ với SanPham
            builder.HasOne(x => x.SanPham)
                   .WithMany(s => s.DanhGias)
                   .HasForeignKey(x => x.MaSP)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}