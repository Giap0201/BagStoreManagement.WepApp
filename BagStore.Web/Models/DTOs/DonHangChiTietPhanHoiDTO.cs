namespace BagStore.Web.Models.DTOs
{
    public class DonHangChiTietPhanHoiDTO
    {
        public int MaChiTietDH { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal ThanhTien => SoLuong * GiaBan;
    }
}
