using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MauSacConfig : IEntityTypeConfiguration<MauSac>
{
    public void Configure(EntityTypeBuilder<MauSac> builder)
    {
        builder.HasKey(e => e.MaMauSac);
        builder.Property(e => e.TenMauSac).IsRequired().HasMaxLength(50);
    }
}