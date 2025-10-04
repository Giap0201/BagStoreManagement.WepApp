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
    public class ChiTietSanPhamConfig : IEntityTypeConfiguration<ChiTietSanPham>
    {
        public void Configure(EntityTypeBuilder<ChiTietSanPham> builder)
        {
            builder.HasKey(x => x.MaChiTietSP);
            //quan he voi san pham
            builder.HasOne(x => x.SanPham)
                .WithMany(s => s.ChiTietSanPhams)
                .HasForeignKey(e => e.MaSP);
            //quan he voi mau sac
            builder.HasOne(x => x.MauSac)
                .WithMany(m => m.ChiTietSanPhams)
                .HasForeignKey(e => e.MaMauSac);

            //quan he voi kich thuoc
            builder.HasOne(x => x.KichThuoc)
                .WithMany(k => k.ChiTietSanPhams)
                .HasForeignKey(e => e.MaKichThuoc);
        }
    }
}