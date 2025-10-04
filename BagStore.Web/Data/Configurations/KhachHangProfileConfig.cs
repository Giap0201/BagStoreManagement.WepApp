using BagStore.Domain.Entities;
using BagStore.Domain.Entities.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class KhachHangProfileConfig : IEntityTypeConfiguration<KhachHangProfile>
{
    public void Configure(EntityTypeBuilder<KhachHangProfile> builder)
    {
        builder.HasKey(kh => kh.UserId);

        // Mối quan hệ 1-1 với ApplicationUser (bắt buộc)
        // Dùng RESTRICT để ngăn chặn ApplicationUser tự động xóa KhachHangProfile, phá vỡ chu trình CASCADE.
        builder.HasOne(e => e.User)
               .WithOne(u => u.KhachHangProfile)
               .HasForeignKey<KhachHangProfile>(e => e.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict); // ✅ FIX LỖI 1785
    }
}