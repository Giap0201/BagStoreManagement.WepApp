using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class ChiTietSanPhamConfig : IEntityTypeConfiguration<ChiTietSanPham>
    {
        public void Configure(EntityTypeBuilder<ChiTietSanPham> builder)
        {
            builder.ToTable("ChiTietSanPham");

            builder.HasKey(x => x.MaChiTietSP);

            builder.Property(x => x.SoLuongTon)
                   .HasDefaultValue(0);
            builder.HasCheckConstraint("CK_ChiTietSanPham_SoLuongTon", "[SoLuongTon] >= 0"); // Check số lượng tồn ≥0

            builder.Property(x => x.GiaBan)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.NgayTao)
                   .HasDefaultValueSql("GETDATE()");

            // Unique constraint tránh trùng biến thể
            builder.HasIndex(x => new { x.MaSP, x.MaKichThuoc, x.MaMauSac })
                   .IsUnique();

            // FK với SanPham, KichThuoc, MauSac
            builder.HasOne(x => x.SanPham)
                   .WithMany(s => s.ChiTietSanPhams)
                   .HasForeignKey(x => x.MaSP)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.KichThuoc)
                   .WithMany(k => k.ChiTietSanPhams)
                   .HasForeignKey(x => x.MaKichThuoc)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MauSac)
                   .WithMany(m => m.ChiTietSanPhams)
                   .HasForeignKey(x => x.MaMauSac)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}