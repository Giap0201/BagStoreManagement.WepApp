using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.DTOs
{
    public class KichThuocDto
    {
        public int MaKichThuoc { get; set; }

        [Required(ErrorMessage = "Tên kích thước không được để trống")]
        [StringLength(50, ErrorMessage = "Tên kích thước tối đa 50 ký tự")]
        public string TenKichThuoc { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Chiều dài phải là số dương")]
        public decimal? ChieuDai { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Chiều rộng phải là số dương")]
        public decimal? ChieuRong { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Chiều cao phải là số dương")]
        public decimal? ChieuCao { get; set; }
    }
}