using BagStore.Domain.Entities;
using BagStore.Domain.Entities.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NhanVienProfileConfig : IEntityTypeConfiguration<NhanVienProfile>
{
    public void Configure(EntityTypeBuilder<NhanVienProfile> builder)
    {
        builder.HasKey(nv => nv.UserId);

        // Mối quan hệ 1-1 với ApplicationUser (bắt buộc)
        // Dùng RESTRICT để ngăn chặn ApplicationUser tự động xóa NhanVienProfile, phá vỡ chu trình CASCADE.
        builder.HasOne(e => e.ApplicationUser)
               .WithOne(u => u.NhanVienProfile)
               .HasForeignKey<NhanVienProfile>(e => e.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict); // ✅ FIX LỖI 1785
    }
}