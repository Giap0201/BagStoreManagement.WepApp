using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;

namespace BagStore.Web.Models.ViewModels
{
    public class SanPhamDetailViewModel
    {
        public SanPhamResponseDto SanPham { get; set; }
        public List<ChiTietSanPhamResponseDto> ChiTietSanPhams { get; set; }
        public List<AnhSanPhamResponseDto> AnhSanPhams { get; set; }
    }
}