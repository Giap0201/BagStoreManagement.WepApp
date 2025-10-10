using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class SanPhamConfig : IEntityTypeConfiguration<SanPham>
    {
        public void Configure(EntityTypeBuilder<SanPham> builder)
        {
            builder.ToTable("SanPham");

            builder.HasKey(x => x.MaSP);

            builder.Property(x => x.TenSP)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.MoTaChiTiet)
                   .HasMaxLength(500);

            builder.Property(x => x.MetaTitle)
                   .HasMaxLength(200);

            builder.Property(x => x.MetaDescription)
                   .HasMaxLength(500);

            builder.Property(x => x.NgayCapNhat)
                   .HasDefaultValueSql("GETDATE()");

            // Quan hệ với bảng DanhMucLoaiTui, ThuongHieu, ChatLieu
            builder.HasOne(x => x.DanhMucLoaiTui)
                   .WithMany(d => d.SanPhams)
                   .HasForeignKey(x => x.MaLoaiTui)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ThuongHieu)
                   .WithMany(t => t.SanPhams)
                   .HasForeignKey(x => x.MaThuongHieu)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ChatLieu)
                   .WithMany(c => c.SanPhams)
                   .HasForeignKey(x => x.MaChatLieu)
                   .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ 1:N với ChiTietSanPham, AnhSanPham, DanhGia
            builder.HasMany(x => x.ChiTietSanPhams)
                   .WithOne(c => c.SanPham)
                   .HasForeignKey(c => c.MaSP)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.AnhSanPhams)
                   .WithOne(a => a.SanPham)
                   .HasForeignKey(a => a.MaSP)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.DanhGias)
                   .WithOne(d => d.SanPham)
                   .HasForeignKey(d => d.MaSP)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}