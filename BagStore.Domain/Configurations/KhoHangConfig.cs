using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class KhoHangConfig : IEntityTypeConfiguration<KhoHang>
{
    public void Configure(EntityTypeBuilder<KhoHang> builder)
    {
        builder.HasKey(e => e.MaKho);
        builder.Property(e => e.TenKho).IsRequired().HasMaxLength(150);
        builder.Property(e => e.DiaChi).HasMaxLength(255);
    }
}