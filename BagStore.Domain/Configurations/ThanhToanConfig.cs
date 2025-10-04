using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ThanhToanConfig : IEntityTypeConfiguration<ThanhToan>
{
    public void Configure(EntityTypeBuilder<ThanhToan> builder)
    {
        builder.HasKey(e => e.MaThanhToan);
        builder.Property(e => e.SoTien).IsRequired().HasColumnType("decimal(18,2)");

        // Mối quan hệ Đơn hàng
        builder.HasOne(tt => tt.DonHang)
               .WithMany(dh => dh.ThanhToans)
               .HasForeignKey(tt => tt.MaDonHang)
               .IsRequired();

        // Mối quan hệ Nhân viên Xác nhận (Nullable)
        builder.HasOne(tt => tt.NhanVienXacNhan)
               .WithMany(nv => nv.ThanhToanDaXacNhans)
               .HasForeignKey(tt => tt.NhanVienXacNhanId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

        // RÀNG BUỘC CHECK cho Trạng thái
        builder.ToTable(tb => tb.HasCheckConstraint("CK_ThanhToan_TrangThai", "TrangThai IN ('Thành công', 'Thất bại', 'Chờ xác nhận', 'Đã hoàn tiền')"));
    }
}