using BagStore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations.Identity
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers"); // giữ nguyên table Identity

            builder.Property(u => u.FullName).HasMaxLength(200);
            builder.Property(u => u.NgaySinh).IsRequired(false);

            // Quan hệ 1-1 với KhachHang
            builder.HasOne(u => u.KhachHang)
                   .WithOne(k => k.ApplicationUser)
                   .HasForeignKey<KhachHang>(k => k.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ 1-1 với NhanVienProfile
            builder.HasOne(u => u.NhanVienProfile)
                   .WithOne(nv => nv.ApplicationUser)
                   .HasForeignKey<NhanVienProfile>(nv => nv.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}