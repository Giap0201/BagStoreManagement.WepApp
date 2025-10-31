using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class ChiTietPhieuNhapConfig : IEntityTypeConfiguration<ChiTietPhieuNhap>
    {
        public void Configure(EntityTypeBuilder<ChiTietPhieuNhap> builder)
        {
            builder.ToTable("ChiTietPhieuNhap");

            builder.HasKey(x => x.MaChiTietNhap);

            builder.Property(x => x.SoLuongNhap)
                   .IsRequired();
            builder.HasCheckConstraint("CK_ChiTietPhieuNhap_SoLuongNhap", "[SoLuongNhap] > 0");

            builder.Property(x => x.DonGia)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.PhieuNhapHang)
                   .WithMany(p => p.ChiTietPhieuNhaps)
                   .HasForeignKey(x => x.MaPhieuNhap)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ChiTietSanPham)
                   .WithMany(c => c.ChiTietPhieuNhaps)
                   .HasForeignKey(x => x.MaChiTietSP)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}