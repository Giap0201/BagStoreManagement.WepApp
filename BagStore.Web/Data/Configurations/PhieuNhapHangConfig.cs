using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PhieuNhapHangConfig : IEntityTypeConfiguration<PhieuNhapHang>
{
    public void Configure(EntityTypeBuilder<PhieuNhapHang> builder)
    {
        builder.HasKey(e => e.MaPhieuNhap);
        builder.Property(e => e.TongTien).IsRequired().HasColumnType("decimal(18,2)");

        // FKs
        builder.HasOne(e => e.NhaCungCap)
            .WithMany(ncc => ncc.PhieuNhapHangs)
            .HasForeignKey(e => e.MaNhaCungCap)
            ;
        builder.HasOne(e => e.NhanVienTao)
            .WithMany(nv => nv.PhieuNhapHangs)
            .HasForeignKey(e => e.NhanVienTaoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}