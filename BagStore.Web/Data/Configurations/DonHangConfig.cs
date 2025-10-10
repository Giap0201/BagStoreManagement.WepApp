using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class DonHangConfig : IEntityTypeConfiguration<DonHang>
    {
        public void Configure(EntityTypeBuilder<DonHang> builder)
        {
            builder.ToTable("DonHang");

            builder.HasKey(x => x.MaDonHang);

            builder.Property(x => x.NgayDatHang)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.TongTien)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TrangThai)
                   .HasMaxLength(50)
                   .HasDefaultValue("Chờ xử lý");
            builder.HasCheckConstraint("CK_DonHang_TrangThai", "[TrangThai] IN ('Chờ xử lý','Hoàn thành','Hủy')");

            builder.Property(x => x.DiaChiGiaoHang)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.PhuongThucThanhToan)
                   .HasMaxLength(50);
            builder.HasCheckConstraint("CK_DonHang_PTTT", "[PhuongThucThanhToan] IN ('COD','Chuyển khoản')");

            builder.Property(x => x.PhiGiaoHang)
                   .HasDefaultValue(0)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TrangThaiThanhToan)
                   .HasMaxLength(50)
                   .HasDefaultValue("Chưa thanh toán");
            builder.HasCheckConstraint("CK_DonHang_ThanhToan", "[TrangThaiThanhToan] IN ('Chưa thanh toán','Thành công','Thất bại')");

            // Quan hệ với KhachHang
            builder.HasOne(x => x.KhachHang)
                   .WithMany(k => k.DonHangs)
                   .HasForeignKey(x => x.MaKH)
                   .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ 1:N với ChiTietDonHang
            builder.HasMany(x => x.ChiTietDonHangs)
                   .WithOne(c => c.DonHang)
                   .HasForeignKey(c => c.MaDonHang)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}