using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class NhaCungCapConfig : IEntityTypeConfiguration<NhaCungCap>
    {
        public void Configure(EntityTypeBuilder<NhaCungCap> builder)
        {
            builder.ToTable("NhaCungCap");

            builder.HasKey(x => x.MaNCC);

            builder.Property(x => x.TenNCC)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.DiaChi)
                   .HasMaxLength(500);

            builder.Property(x => x.SoDienThoai)
                   .HasMaxLength(15);

            builder.Property(x => x.Email)
                   .HasMaxLength(150);

            // Quan hệ 1:N với PhieuNhapHang
            builder.HasMany(x => x.PhieuNhapHangs)
                   .WithOne(p => p.NhaCungCap)
                   .HasForeignKey(p => p.MaNCC)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}