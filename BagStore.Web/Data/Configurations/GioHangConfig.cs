using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GioHangConfig : IEntityTypeConfiguration<GioHang>
{
    public void Configure(EntityTypeBuilder<GioHang> builder)
    {
        builder.HasKey(e => e.MaGioHang);
        builder.Property(e => e.SoLuong).IsRequired();
        builder.Property(e => e.TrangThai).HasDefaultValue("Hoạt động").HasMaxLength(50);

        // Unique Index: Đảm bảo 1 khách hàng chỉ có 1 mục cho 1 biến thể SP

        // FKs
        builder.HasOne(e => e.KhachHangProfile)
            .WithMany(kp => kp.GioHangs)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.ChiTietSanPham)
            .WithMany(ct => ct.GioHangs)
            .HasForeignKey(e => e.MaChiTietSanPham);

        // RÀNG BUỘC CHECK
        builder.ToTable(tb => tb.HasCheckConstraint("CK_GioHang_SoLuong", "SoLuong > 0"));
        builder.ToTable(tb => tb.HasCheckConstraint("CK_GioHang_TrangThai", "TrangThai IN ('Hoạt động', 'Đã chuyển đơn', 'Đã xóa')"));
    }
}