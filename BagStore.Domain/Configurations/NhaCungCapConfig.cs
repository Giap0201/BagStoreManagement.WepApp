using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NhaCungCapConfig : IEntityTypeConfiguration<NhaCungCap>
{
    public void Configure(EntityTypeBuilder<NhaCungCap> builder)
    {
        builder.HasKey(e => e.MaNhaCungCap);
        builder.Property(e => e.TenNhaCungCap).IsRequired().HasMaxLength(255);
        builder.Property(e => e.SoDienThoai).HasMaxLength(15);
    }
}