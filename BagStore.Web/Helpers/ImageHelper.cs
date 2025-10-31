using BagStore.Domain.Entities;

namespace BagStore.Web.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
        /// Upload danh sách IFormFile lên server và trả về danh sách AnhSanPham
        /// </summary>
        /// <param name="files">Danh sách file upload</param>
        /// <param name="webRootPath">Đường dẫn wwwroot</param>
        /// <param name="hinhChinhIndex">Vị trí ảnh chính (mặc định = 0)</param>
        /// <param name="thuTuHienThiStart">Thứ tự hiển thị bắt đầu (dùng khi merge ảnh cũ + mới)</param>
        /// <returns>Danh sách AnhSanPham đã upload</returns>
        public static async Task<List<AnhSanPham>> UploadImagesAsync(
            List<IFormFile> files,
            string webRootPath,
            int hinhChinhIndex = 0,
            int thuTuHienThiStart = 1)
        {
            var result = new List<AnhSanPham>();

            if (files == null || files.Count == 0)
                return result;

            string uploadFolder = Path.Combine(webRootPath ?? "wwwroot", "uploads");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];

                // Tạo tên file an toàn, tránh trùng lặp
                string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                string filePath = Path.Combine(uploadFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                result.Add(new AnhSanPham
                {
                    DuongDan = $"/uploads/{fileName}",
                    ThuTuHienThi = thuTuHienThiStart + i,
                    LaHinhChinh = i == hinhChinhIndex
                });
            }

            return result;
        }

        /// <summary>
        /// Upload 1 ảnh duy nhất (ví dụ ảnh chính) và trả về AnhSanPham
        /// </summary>
        public static async Task<AnhSanPham> UploadSingleImageAsync(
            IFormFile file,
            string webRootPath,
            int thuTuHienThi = 1,
            bool laHinhChinh = true)
        {
            if (file == null)
                return null;

            string uploadFolder = Path.Combine(webRootPath ?? "wwwroot", "uploads");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return new AnhSanPham
            {
                DuongDan = $"/uploads/{fileName}",
                ThuTuHienThi = thuTuHienThi,
                LaHinhChinh = laHinhChinh
            };
        }
    }
}