using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TonKhoConfig : IEntityTypeConfiguration<TonKho>
{
    public void Configure(EntityTypeBuilder<TonKho> builder)
    {
        builder.HasKey(e => e.MaTonKho);
        builder.Property(e => e.SoLuongTon).IsRequired().HasDefaultValue(0);

        // FKs đến ChiTietSanPham và KhoHang
        builder.HasOne(t => t.ChiTietSanPham).WithMany(ct => ct.TonKhos).HasForeignKey(t => t.MaChiTietSanPham);
        builder.HasOne(t => t.KhoHang).WithMany(k => k.TonKhos).HasForeignKey(t => t.MaKho);
    }
}