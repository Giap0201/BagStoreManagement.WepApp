using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DonHangConfig : IEntityTypeConfiguration<DonHang>
{
    public void Configure(EntityTypeBuilder<DonHang> builder)
    {
        builder.HasKey(dh => dh.MaDonHang);
        builder.Property(dh => dh.TongGiaTriHang).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(dh => dh.GiamGiaTong).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(dh => dh.TongTien).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(dh => dh.PhiGiaoHang).HasColumnType("decimal(18,2)").HasDefaultValue(0);

        // Mối quan hệ Khách hàng (Bắt buộc)
        builder.HasOne(dh => dh.KhachHangProfile)
               .WithMany(kp => kp.DonHangs)
               .HasForeignKey(dh => dh.UserId)
               .IsRequired().OnDelete(DeleteBehavior.Restrict); // Ngăn chặn xóa Khách hàng nếu còn đơn hàng

        // Mối quan hệ Nhân viên Xử lý (Nullable)
        builder.HasOne(dh => dh.NhanVienXuLy)
               .WithMany(nv => nv.DonHangDaXuLys)
               .HasForeignKey(dh => dh.NhanVienXuLyId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull); // Nếu nhân viên bị xóa, giữ đơn hàng nhưng đặt trường này thành null

        // RÀNG BUỘC CHECK cho Trạng thái
        builder.ToTable(tb => tb.HasCheckConstraint("CK_DonHang_TrangThai", "TrangThai IN ('Chờ xử lý', 'Đang giao hàng', 'Hoàn thành', 'Đã hủy')"));
        builder.ToTable(tb => tb.HasCheckConstraint("CK_DonHang_PhuongThucTT", "PhuongThucThanhToan IN ('COD', 'Chuyển khoản', 'Ví điện tử')"));
    }
}