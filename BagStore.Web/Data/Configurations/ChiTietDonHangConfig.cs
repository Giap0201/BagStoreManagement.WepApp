using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class ChiTietDonHangConfig : IEntityTypeConfiguration<ChiTietDonHang>
    {
        public void Configure(EntityTypeBuilder<ChiTietDonHang> builder)
        {
            builder.ToTable("ChiTietDonHang");

            builder.HasKey(x => x.MaChiTietDH);

            builder.Property(x => x.SoLuong)
                   .IsRequired();
            builder.HasCheckConstraint("CK_ChiTietDonHang_SoLuong", "[SoLuong] > 0");

            builder.Property(x => x.GiaBan)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.DonHang)
                   .WithMany(d => d.ChiTietDonHangs)
                   .HasForeignKey(x => x.MaDonHang)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ChiTietSanPham)
                   .WithMany(c => c.ChiTietDonHangs)
                   .HasForeignKey(x => x.MaChiTietSP)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}