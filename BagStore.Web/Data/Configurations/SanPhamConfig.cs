using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Configurations
{
    public class SanPhamConfig : IEntityTypeConfiguration<SanPham>
    {
        public void Configure(EntityTypeBuilder<SanPham> builder)
        {
            builder.HasKey(x => x.MaSP);
            builder.Property(x => x.TenSP).IsRequired().HasMaxLength(200);
            builder.Property(x => x.GiaBan).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.GiaGoc).HasColumnType("decimal(18,2)");
            builder.Property(x => x.NhanVienCapNhatId).IsRequired().HasMaxLength(450);

            //FK den danh muc thuong hieu chat lieu
            builder.HasOne(x => x.DanhMucLoaiTui)
                .WithMany(d => d.SanPhams)
                .HasForeignKey(e => e.MaLoaiTui);
            builder.HasOne(x => x.ThuongHieu)
                .WithMany(t => t.SanPhams)
                .HasForeignKey(e => e.MaThuongHieu);
            builder.HasOne(x => x.ChatLieu)
                .WithMany(s => s.SanPhams)
                .HasForeignKey(e => e.MaChatLieu);

            //FK den nhan vien
            builder.HasOne(x => x.NhanVienCapNhat)
                .WithMany(nv => nv.SanPhamDaCapNhats)
                .HasForeignKey(e => e.NhanVienCapNhatId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}