using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ChiTietPhieuNhapConfig : IEntityTypeConfiguration<ChiTietPhieuNhap>
{
    public void Configure(EntityTypeBuilder<ChiTietPhieuNhap> builder)
    {
        // Khóa Tổng Hợp
        builder.HasKey(e => new { e.MaPhieuNhap, e.MaChiTietPhieuNhap });

        builder.Property(e => e.DonGia).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.SoLuongNhap).IsRequired();

        // FKs
        builder.HasOne(e => e.PhieuNhapHang).WithMany(pnh => pnh.ChiTietPhieuNhaps).HasForeignKey(e => e.MaPhieuNhap);
        builder.HasOne(e => e.ChiTietSanPham).WithMany(ct => ct.ChiTietPhieuNhaps).HasForeignKey(e => e.MaChiTietSanPham);

        builder.ToTable(tb => tb.HasCheckConstraint("CK_ChiTietPhieuNhap_SoLuongNhap", "SoLuongNhap > 0"));
    }
}