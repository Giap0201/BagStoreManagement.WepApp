namespace BagStore.Web.Models.DTOs.Responses
{
    public class CartItemResponse
    {
        public int MaSP_GH { get; set; }
        public string TenSP { get; set; } = string.Empty;
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }
        public string MauSac { get; set; } = string.Empty;
        public string KichThuoc { get; set; } = string.Empty;
        public string DuongDanAnh { get; set; } = string.Empty;
        public decimal ThanhTien { get; set; }
    }

    public class CartResponse
    {
        public int MaKH { get; set; }
        public List<CartItemResponse> Items { get; set; } = new();
        public decimal TongTien => Items.Sum(i => i.ThanhTien);
    }
}
