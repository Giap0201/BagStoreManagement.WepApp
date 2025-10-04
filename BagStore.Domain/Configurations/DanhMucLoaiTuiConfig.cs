using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DanhMucLoaiTuiConfig : IEntityTypeConfiguration<DanhMucLoaiTui>
{
    public void Configure(EntityTypeBuilder<DanhMucLoaiTui> builder)
    {
        builder.HasKey(e => e.MaLoaiTui);
        builder.Property(e => e.TenLoaiTui).IsRequired().HasMaxLength(150);
        builder.Property(e => e.ThuTuHienThi).HasDefaultValue(0);
    }
}