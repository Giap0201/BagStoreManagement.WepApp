using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ChiTietDonHangConfig : IEntityTypeConfiguration<ChiTietDonHang>
{
    public void Configure(EntityTypeBuilder<ChiTietDonHang> builder)
    {
        // Khóa Tổng Hợp (Đảm bảo 1 sản phẩm chỉ xuất hiện 1 lần trong 1 đơn hàng)
        builder.HasKey(e => new { e.MaDonHang, e.MaChiTietSanPham });

        builder.Property(e => e.GiaBan).IsRequired().HasColumnType("decimal(18,2)"); // Snapshot Price
        builder.Property(e => e.SoLuong).IsRequired();

        // FKs
        builder.HasOne(e => e.DonHang).WithMany(dh => dh.ChiTietDonHangs).HasForeignKey(e => e.MaDonHang);
        builder.HasOne(e => e.ChiTietSanPham).WithMany(ct => ct.ChiTietDonHangs).HasForeignKey(e => e.MaChiTietSanPham);

        builder.ToTable(tb => tb.HasCheckConstraint("CK_ChiTietDonHang_SoLuong", "SoLuong > 0"));
    }
}