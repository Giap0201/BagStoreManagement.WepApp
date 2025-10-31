using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Helpers;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;
using NuGet.Common;

namespace BagStore.Web.Services.Implementations
{
    public class AnhSanPhamService : IAnhSanPhamService
    {
        private readonly IAnhSanPhamRepository _repo;
        private readonly ISanPhamRepository _repoSanPham;
        private readonly IWebHostEnvironment _env;

        public AnhSanPhamService(IAnhSanPhamRepository repo,
            ISanPhamRepository repoSanPham,
            IWebHostEnvironment env)
        {
            _repo = repo;
            _repoSanPham = repoSanPham;
            _env = env;
        }

        //them moi anh san pham
        public async Task<BaseResponse<AnhSanPhamResponseDto>> CreateAsync(AnhSanPhamRequestDto dto)
        {
            if (dto == null)
                return BaseResponse<AnhSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("Dto", "Dữ liệu không được null") },
                    "Tạo ảnh thất bại");

            var sp = await _repoSanPham.GetByIdAsync(dto.MaSP);
            if (sp == null)
            {
                return BaseResponse<AnhSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaSP", "Sản phẩm không tồn tại") },
                    "Tạo ảnh thất bại");
            }
            //lay tat ca anh hien co cua san pham
            var allImages = await _repo.GetBySanPhamIdAsync(dto.MaSP);
            //kiem tra trung thu tu hien thi
            if (allImages.Any(a => a.ThuTuHienThi == dto.ThuTuHienThi))
            {
                return BaseResponse<AnhSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("ThuTuHienThi", "Thứ tự hiển thị đã tồn tại") },
                    "Tạo ảnh thất bại");
            }

            //xu li anh chinh

            if (dto.LaHinhChinh)
            {
                //tim anh chinh cu neu co
                var oldPrimary = allImages.FirstOrDefault(a => a.LaHinhChinh);
                if (oldPrimary != null)
                {
                    oldPrimary.LaHinhChinh = false;
                    await _repo.UpdateAsync(oldPrimary);
                }
            }
            // upload file
            var anh = await ImageHelper.UploadSingleImageAsync(dto.FileAnh, _env.WebRootPath, dto.ThuTuHienThi, dto.LaHinhChinh);
            anh.MaSP = dto.MaSP;

            var created = await _repo.AddAsync(anh);
            return BaseResponse<AnhSanPhamResponseDto>.Success(MapEntityToResponse(created), "Thêm ảnh thành công");
        }

        //cap nhat thong tin anh (chi thu tu hien thi va la hinh chinh)
        public async Task<BaseResponse<AnhSanPhamResponseDto>> UpdateAsync(int maAnh, AnhSanPhamRequestDto dto)
        {
            if (dto == null)
                return BaseResponse<AnhSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("Dto", "Dữ liệu không được null") },
                    "Cập nhật thất bại");

            var entity = await _repo.GetByIdAsync(maAnh);
            if (entity == null)
                return BaseResponse<AnhSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaAnh", "Không tìm thấy ảnh sản phẩm") },
                    "Cập nhật thất bại");

            var allOtherImages = (await _repo.GetBySanPhamIdAsync(entity.MaSP))
                                 .Where(a => a.MaAnh != maAnh);
            //lay tat ca anh hien co cua san pham
            if (allOtherImages.Any(a => a.ThuTuHienThi == dto.ThuTuHienThi))
            {
                return BaseResponse<AnhSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("ThuTuHienThi", $"Thứ tự {dto.ThuTuHienThi} đã tồn tại.") },
                    "Cập nhật thất bại");
            }

            // Xử lý ảnh chính (chỉ khi set ảnh MỚI làm chính)
            if (dto.LaHinhChinh && !entity.LaHinhChinh)
            {
                var oldPrimary = allOtherImages.FirstOrDefault(a => a.LaHinhChinh);
                if (oldPrimary != null)
                {
                    oldPrimary.LaHinhChinh = false;
                    await _repo.UpdateAsync(oldPrimary);
                }
            }

            if (dto.FileAnh != null)
            {
                var anhMoi = await ImageHelper.UploadSingleImageAsync(dto.FileAnh, _env.WebRootPath, dto.ThuTuHienThi, dto.LaHinhChinh);
                entity.DuongDan = anhMoi.DuongDan;
            }

            entity.ThuTuHienThi = dto.ThuTuHienThi;
            entity.LaHinhChinh = dto.LaHinhChinh;

            var updated = await _repo.UpdateAsync(entity);

            return BaseResponse<AnhSanPhamResponseDto>.Success(MapEntityToResponse(updated), "Cập nhật ảnh thành công");
        }

        //phuong thuc xoa anh
        public async Task<BaseResponse<bool>> DeleteAsync(int maAnh)
        {
            var entity = await _repo.GetByIdAsync(maAnh);
            if (entity == null)
                return BaseResponse<bool>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaAnh", "Ảnh không tồn tại") },
                    "Xóa thất bại");

            if (entity.LaHinhChinh)
            {
                // Tìm 1 ảnh khác để thay thế
                var allOtherImages = (await _repo.GetBySanPhamIdAsync(entity.MaSP))
                                       .Where(a => a.MaAnh != maAnh);

                // Ưu tiên ảnh có thứ tự nhỏ nhất
                var newPrimary = allOtherImages.OrderBy(a => a.ThuTuHienThi).FirstOrDefault();

                if (newPrimary != null)
                {
                    // Đặt ảnh này làm chính
                    newPrimary.LaHinhChinh = true;
                    await _repo.UpdateAsync(newPrimary); // Chỉ cần cập nhật ảnh này
                }
                // Nếu không còn ảnh nào, không cần làm gì cả
            }
            var success = await _repo.DeleteAsync(maAnh); // Xóa ảnh
            return BaseResponse<bool>.Success(success, success ? "Xóa ảnh thành công" : "Xóa thất bại");
        }

        //lay anh san pham theo ma
        public async Task<BaseResponse<AnhSanPhamResponseDto>> GetByIdAsync(int maAnh)
        {
            var entity = await _repo.GetByIdAsync(maAnh);
            if (entity == null)
                return BaseResponse<AnhSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaAnh", "Không tìm thấy") },
                    "Lấy dữ liệu thất bại");

            return BaseResponse<AnhSanPhamResponseDto>.Success(MapEntityToResponse(entity), "Lấy ảnh thành công");
        }

        //lay anh san pham theo ma san pham
        public async Task<BaseResponse<List<AnhSanPhamResponseDto>>> GetBySanPhamIdAsync(int maSP)
        {
            var entities = await _repo.GetBySanPhamIdAsync(maSP);
            var dtos = entities.Select(MapEntityToResponse).ToList();
            return BaseResponse<List<AnhSanPhamResponseDto>>.Success(dtos, "Lấy danh sách ảnh thành công");
        }

        private AnhSanPhamResponseDto MapEntityToResponse(AnhSanPham entity)
        {
            return new AnhSanPhamResponseDto
            {
                MaAnh = entity.MaAnh,
                DuongDan = entity.DuongDan,
                ThuTuHienThi = entity.ThuTuHienThi,
                LaHinhChinh = entity.LaHinhChinh
            };
        }

        public Task<BaseResponse<List<AnhSanPham>>> CreateMultipleAsync(int maSP, List<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<List<AnhSanPhamResponseDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<AnhSanPhamResponseDto>> SetPrimaryAsync(int maAnh)
        {
            var entity = await _repo.GetByIdAsync(maAnh);
            if (entity == null)
            {
                return BaseResponse<AnhSanPhamResponseDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaAnh", "Ảnh không tồn tại") },
                    "Thất bại");
            }

            if (entity.LaHinhChinh)
            {
                return BaseResponse<AnhSanPhamResponseDto>.Success(MapEntityToResponse(entity), "Ảnh đã là ảnh chính.");
            }
            // 1. Lấy ảnh chính cũ
            var allImages = await _repo.GetBySanPhamIdAsync(entity.MaSP);
            var oldPrimary = allImages.FirstOrDefault(a => a.MaAnh != maAnh && a.LaHinhChinh);

            // 2. Bỏ cờ ảnh cũ (nếu có)
            if (oldPrimary != null)
            {
                oldPrimary.LaHinhChinh = false;
                await _repo.UpdateAsync(oldPrimary);
            }

            // 3. Set ảnh mới
            entity.LaHinhChinh = true;
            var updated = await _repo.UpdateAsync(entity);
            // Trả về DTO của ảnh vừa được set (JavaScript cần 'duongDan' để cập nhật)
            return BaseResponse<AnhSanPhamResponseDto>.Success(MapEntityToResponse(updated), "Đặt ảnh chính thành công");
        }
    }
}