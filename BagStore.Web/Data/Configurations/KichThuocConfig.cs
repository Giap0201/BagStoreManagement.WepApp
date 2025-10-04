using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class KichThuocConfig : IEntityTypeConfiguration<KichThuoc>
{
    public void Configure(EntityTypeBuilder<KichThuoc> builder)
    {
        builder.HasKey(e => e.MaKichThuoc);
        builder.Property(e => e.TenKichThuoc).IsRequired().HasMaxLength(50);
    }
}