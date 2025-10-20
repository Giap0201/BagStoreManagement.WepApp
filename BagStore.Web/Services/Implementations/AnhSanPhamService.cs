using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Helpers;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;

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

            //neu co file moi, upload va thay duong dan
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

            var success = await _repo.DeleteAsync(maAnh);
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
    }
}