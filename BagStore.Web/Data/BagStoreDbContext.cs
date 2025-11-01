using BagStore.Data.Configurations;
using BagStore.Domain.Entities;
using BagStore.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Data
{
    // BagStoreDbContext kế thừa IdentityDbContext để tích hợp Identity
    // ApplicationUser là user tùy chỉnh của hệ thống, quản lý Admin và Client
    public class BagStoreDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public BagStoreDbContext(DbContextOptions<BagStoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<DanhMucLoaiTui> DanhMucLoaiTuis { get; set; }
        public DbSet<ThuongHieu> ThuongHieux { get; set; }
        public DbSet<ChatLieu> ChatLieus { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<KichThuoc> KichThuocs { get; set; }

        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<ChiTietSanPham> ChiTietSanPhams { get; set; }
        public DbSet<AnhSanPham> AnhSanPhams { get; set; }

        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVienProfile> NhanVienProfiles { get; set; }

        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<PhieuNhapHang> PhieuNhapHangs { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

        public DbSet<DanhGia> DanhGias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Quan trọng để Identity hoạt động

            // ✅ Giữ lại phần cấu hình tự động
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BagStoreDbContext).Assembly);

            // ✅ Thêm check constraint cho DonHang
            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.ToTable("DonHang", t =>
                {
                    t.HasCheckConstraint("CK_DonHang_PTTT", "[PhuongThucThanhToan] IN (N'COD', N'Chuyển khoản', N'Ví điện tử')");
                    t.HasCheckConstraint("CK_DonHang_ThanhToan", "[TrangThaiThanhToan] IN (N'Thành công', N'Thất bại', N'Chờ xác nhận', N'Đã hoàn tiền')");
                    t.HasCheckConstraint("CK_DonHang_TrangThai", "[TrangThai] IN (N'Chờ xử lý', N'Đang giao hàng', N'Hoàn thành', N'Đã huỷ')");
                });
            });

            // ✅ (tuỳ chọn) nếu bạn muốn seed user/role thì để phần HasData ở đây
        }

    }
}