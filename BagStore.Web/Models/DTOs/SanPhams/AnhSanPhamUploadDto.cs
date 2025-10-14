using System.ComponentModel.DataAnnotations;

namespace BagStore.Web.Models.DTOs.SanPhams
{
    public class AnhSanPhamUploadDto
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public int ThuTuHienThi { get; set; }

        public bool LaHinhChinh { get; set; } = false;
    }
}