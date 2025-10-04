using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ThuongHieuConfig : IEntityTypeConfiguration<ThuongHieu>
{
    public void Configure(EntityTypeBuilder<ThuongHieu> builder)
    {
        builder.HasKey(e => e.MaThuongHieu);
        builder.Property(e => e.TenThuongHieu).IsRequired().HasMaxLength(150);
    }
}