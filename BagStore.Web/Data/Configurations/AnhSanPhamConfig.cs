using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AnhSanPhamConfig : IEntityTypeConfiguration<AnhSanPham>
{
    public void Configure(EntityTypeBuilder<AnhSanPham> builder)
    {
        builder.HasKey(e => e.MaAnh);
        builder.Property(e => e.DuongDan).IsRequired().HasMaxLength(500);
        builder.Property(e => e.HinhChinh).HasDefaultValue(false);

        // Mối quan hệ với SanPham (Nullable)
        builder.HasOne(e => e.SanPham).WithMany(s => s.AnhSanPhams).HasForeignKey(e => e.MaSanPham).IsRequired(false);

        // Mối quan hệ với ChiTietSanPham (Nullable)
        builder.HasOne(e => e.ChiTietSanPham).WithMany(ct => ct.AnhSanPhams).HasForeignKey(e => e.MaChiTietSanPham).IsRequired(false);

        // Cân nhắc: Nên có một ràng buộc ở tầng ứng dụng hoặc trigger để đảm bảo chỉ có 1 trong 2 FK có giá trị.
    }
}