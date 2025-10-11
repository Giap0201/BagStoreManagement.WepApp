using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class KichThuocConfig : IEntityTypeConfiguration<KichThuoc>
    {
        public void Configure(EntityTypeBuilder<KichThuoc> builder)
        {
            builder.ToTable("KichThuoc");

            builder.HasKey(x => x.MaKichThuoc);

            builder.Property(x => x.TenKichThuoc)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.HasIndex(x => x.TenKichThuoc).IsUnique();

            builder.Property(x => x.ChieuDai).HasColumnType("decimal(5,2)").IsRequired(false);
            builder.Property(x => x.ChieuRong).HasColumnType("decimal(5,2)").IsRequired(false);
            builder.Property(x => x.ChieuCao).HasColumnType("decimal(5,2)").IsRequired(false);

            builder.HasMany(x => x.ChiTietSanPhams)
                   .WithOne(x => x.KichThuoc)
                   .HasForeignKey(x => x.MaKichThuoc)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}