namespace BagStore.Web.Models.ViewModels.SanPhams
{
    public class SanPhamDetailViewModel
    {
        public SanPhamResponseDto SanPham { get; set; }
        public List<ChiTietSanPhamResponseDto> ChiTietSanPhams { get; set; }
        public List<AnhSanPhamResponseDto> AnhSanPhams { get; set; }
    }
}