using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class ThuongHieuConfig : IEntityTypeConfiguration<ThuongHieu>
    {
        public void Configure(EntityTypeBuilder<ThuongHieu> builder)
        {
            builder.ToTable("ThuongHieu");

            builder.HasKey(x => x.MaThuongHieu);

            builder.Property(x => x.TenThuongHieu)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasIndex(x => x.TenThuongHieu).IsUnique();

            builder.Property(x => x.QuocGia)
                   .HasMaxLength(50);

            builder.HasMany(x => x.SanPhams)
                   .WithOne(x => x.ThuongHieu)
                   .HasForeignKey(x => x.MaThuongHieu)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}