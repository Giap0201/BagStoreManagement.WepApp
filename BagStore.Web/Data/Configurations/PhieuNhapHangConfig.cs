using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class PhieuNhapHangConfig : IEntityTypeConfiguration<PhieuNhapHang>
    {
        public void Configure(EntityTypeBuilder<PhieuNhapHang> builder)
        {
            builder.ToTable("PhieuNhapHang");

            builder.HasKey(x => x.MaPhieuNhap);

            builder.Property(x => x.NgayNhap)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.TongTienNhap)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.NhaCungCap)
                   .WithMany(n => n.PhieuNhapHangs)
                   .HasForeignKey(x => x.MaNCC)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ChiTietPhieuNhaps)
                   .WithOne(c => c.PhieuNhapHang)
                   .HasForeignKey(c => c.MaPhieuNhap)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}