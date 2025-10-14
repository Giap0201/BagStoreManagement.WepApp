using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class GioHangConfig : IEntityTypeConfiguration<GioHang>
    {
        public void Configure(EntityTypeBuilder<GioHang> builder)
        {
            builder.ToTable("GioHang");

            builder.HasKey(x => x.MaGioHang);

            builder.Property(x => x.SoLuong)
                   .IsRequired();
            builder.HasCheckConstraint("CK_GioHang_SoLuong", "[SoLuong] > 0");

            builder.Property(x => x.NgayThem)
                   .HasDefaultValueSql("GETDATE()");

            // Unique constraint: tránh trùng item theo KhachHang hoặc SessionID
            builder.HasIndex(x => new { x.MaKH, x.MaChiTietSP }).IsUnique()
                   .HasFilter("[MaKH] IS NOT NULL");

            builder.HasOne(x => x.KhachHang)
                   .WithMany(k => k.GioHangs)
                   .HasForeignKey(x => x.MaKH)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ChiTietSanPham)
                   .WithMany(c => c.GioHangs)
                   .HasForeignKey(x => x.MaChiTietSP)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}