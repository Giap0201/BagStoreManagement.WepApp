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
            builder.HasKey(x => new { x.MaSP, x.MaMauSac, x.MaKichThuoc });
            //quan he voi san pham
            builder.HasOne(x => x.SanPham)
                .WithMany(s => s.ChiTietSanPhams)
                .HasForeignKey(e => e.MaSP)
                .OnDelete(DeleteBehavior.Cascade); //xoa san pham thi xoa chi tiet san pham
            //quan he voi mau sac
            builder.HasOne(x => x.MauSac)
                .WithMany(m => m.ChiTietSanPhams)
                .HasForeignKey(e => e.MaMauSac)
                .OnDelete(DeleteBehavior.Restrict); //khong the xoa mau sac neu con chi tiet san pham
            //quan he voi kich thuoc
            builder.HasOne(x => x.KichThuoc)
                .WithMany(k => k.ChiTietSanPhams)
                .HasForeignKey(e => e.MaKichThuoc)
                .OnDelete(DeleteBehavior.Restrict); //khong the xoa kich thuoc neu con chi tiet san pham
        }
    }
}