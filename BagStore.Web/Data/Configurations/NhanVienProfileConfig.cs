using BagStore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class NhanVienProfileConfig : IEntityTypeConfiguration<NhanVienProfile>
    {
        public void Configure(EntityTypeBuilder<NhanVienProfile> builder)
        {
            builder.ToTable("NhanVienProfile");

            builder.HasKey(nv => nv.Id);

            builder.Property(nv => nv.MaNhanVien)
                   .IsRequired()
                   .HasMaxLength(20);
            builder.HasIndex(nv => nv.MaNhanVien).IsUnique();

            builder.Property(nv => nv.ChucVu)
                   .HasMaxLength(50);

            builder.Property(nv => nv.DiaChi)
                   .HasMaxLength(500);

            builder.Property(nv => nv.SoDienThoai)
                   .HasMaxLength(15);

            builder.HasIndex(nv => nv.ApplicationUserId).IsUnique();
        }
    }
}