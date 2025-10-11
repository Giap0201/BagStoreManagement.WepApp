using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class DanhMucLoaiTuiConfig : IEntityTypeConfiguration<DanhMucLoaiTui>
    {
        public void Configure(EntityTypeBuilder<DanhMucLoaiTui> builder)
        {
            builder.ToTable("DanhMucLoaiTui");

            builder.HasKey(x => x.MaLoaiTui);

            builder.Property(x => x.TenLoaiTui)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasIndex(x => x.TenLoaiTui).IsUnique();

            builder.Property(x => x.MoTa)
                   .HasMaxLength(500);

            builder.HasMany(x => x.SanPhams)
                   .WithOne(x => x.DanhMucLoaiTui)
                   .HasForeignKey(x => x.MaLoaiTui)
                   .OnDelete(DeleteBehavior.Restrict); // Không xóa loại túi nếu còn sản phẩm
        }
    }
}