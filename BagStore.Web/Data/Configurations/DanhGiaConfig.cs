using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DanhGiaConfig : IEntityTypeConfiguration<DanhGia>
{
    public void Configure(EntityTypeBuilder<DanhGia> builder)
    {
        builder.HasKey(e => e.MaDanhGia);

        // FKs đến KhachHangProfile và SanPham
        builder.HasOne(d => d.KhachHangProfile)
            .WithMany(kp => kp.DanhGias)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.SanPham).WithMany(s => s.DanhGias).HasForeignKey(d => d.MaSanPham);
    }
}