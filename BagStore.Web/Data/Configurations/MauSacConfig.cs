using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class MauSacConfig : IEntityTypeConfiguration<MauSac>
    {
        public void Configure(EntityTypeBuilder<MauSac> builder)
        {
            builder.ToTable("MauSac");

            builder.HasKey(x => x.MaMauSac);

            builder.Property(x => x.TenMauSac)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.HasIndex(x => x.TenMauSac).IsUnique();

            builder.HasMany(x => x.ChiTietSanPhams)
                   .WithOne(x => x.MauSac)
                   .HasForeignKey(x => x.MaMauSac)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}