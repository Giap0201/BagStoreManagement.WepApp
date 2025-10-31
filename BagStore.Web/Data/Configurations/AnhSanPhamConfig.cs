using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class AnhSanPhamConfig : IEntityTypeConfiguration<AnhSanPham>
    {
        public void Configure(EntityTypeBuilder<AnhSanPham> builder)
        {
            builder.ToTable("AnhSanPham");

            builder.HasKey(x => x.MaAnh);

            builder.Property(x => x.DuongDan)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.ThuTuHienThi)
                   .HasDefaultValue(0);

            builder.Property(x => x.LaHinhChinh)
                   .HasDefaultValue(false);

            builder.HasOne(x => x.SanPham)
                   .WithMany(s => s.AnhSanPhams)
                   .HasForeignKey(x => x.MaSP)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.MaSP); // Index để tìm kiếm nhanh
        }
    }
}