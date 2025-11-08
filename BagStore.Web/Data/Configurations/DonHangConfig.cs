using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BagStore.Data.Configurations
{
    public class DonHangConfig : IEntityTypeConfiguration<DonHang>
    {
        public void Configure(EntityTypeBuilder<DonHang> builder)
        {
            // Đặt tên bảng và cấu hình Check Constraint
            builder.ToTable("DonHang", t =>
            {
                t.HasCheckConstraint(
                    "CK_DonHang_TrangThai",
                    "[TrangThai] IN (N'Chờ xử lý', N'Đang giao hàng', N'Hoàn thành', N'Đã huỷ')"
                );

                t.HasCheckConstraint(
                    "CK_DonHang_PTTT",
                    "[PhuongThucThanhToan] IN (N'COD', N'Chuyển khoản', N'Ví điện tử')"
                );

                t.HasCheckConstraint(
                    "CK_DonHang_ThanhToan",
                    "[TrangThaiThanhToan] IN (N'Thành công', N'Thất bại', N'Chờ xác nhận', N'Đã hoàn tiền')"
                );
            });

            // 🔑 Khóa chính
            builder.HasKey(x => x.MaDonHang);

            // 🕓 Ngày đặt hàng
            builder.Property(x => x.NgayDatHang)
                   .HasDefaultValueSql("GETDATE()");

            // 💰 Tổng tiền
            builder.Property(x => x.TongTien)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // 📦 Trạng thái đơn hàng
            builder.Property(x => x.TrangThai)
                   .HasMaxLength(50)
                   .HasDefaultValue("Chờ xử lý");

            // 🏠 Địa chỉ giao hàng
            builder.Property(x => x.DiaChiGiaoHang)
                   .IsRequired()
                   .HasMaxLength(500);

            // 💳 Phương thức thanh toán
            builder.Property(x => x.PhuongThucThanhToan)
                   .HasMaxLength(50);

            // 🚚 Phí giao hàng
            builder.Property(x => x.PhiGiaoHang)
                   .HasDefaultValue(0)
                   .HasColumnType("decimal(18,2)");

            // 💵 Trạng thái thanh toán
            builder.Property(x => x.TrangThaiThanhToan)
                   .HasMaxLength(50)
                   .HasDefaultValue("Chưa thanh toán");

            // 👤 Quan hệ: 1 khách hàng - nhiều đơn hàng
            builder.HasOne(x => x.KhachHang)
                   .WithMany(k => k.DonHangs)
                   .HasForeignKey(x => x.MaKH)
                   .OnDelete(DeleteBehavior.Restrict);

            // 📦 Quan hệ: 1 đơn hàng - nhiều chi tiết đơn hàng
            builder.HasMany(x => x.ChiTietDonHangs)
                   .WithOne(c => c.DonHang)
                   .HasForeignKey(c => c.MaDonHang)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}