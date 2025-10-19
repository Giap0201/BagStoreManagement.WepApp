using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.Common;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;

namespace BagStore.Web.Services.Implementations
{
    public class ChatLieuService : IChatLieuService
    {
        private readonly IChatLieuRepository _repo;

        public ChatLieuService(IChatLieuRepository repo)
        {
            _repo = repo;
        }

        // Thêm mới chất liệu
        public async Task<BaseResponse<ChatLieuDto>> CreateAsync(ChatLieuDto dto)
        {
            // Kiểm tra duplicate tên
            var existing = await _repo.GetByNameAsync(dto.TenChatLieu);
            if (existing != null)
                return BaseResponse<ChatLieuDto>.Error(
                    new List<ErrorDetail>
                    {
                        new ErrorDetail(nameof(dto.TenChatLieu), $"Tên chất liệu '{dto.TenChatLieu}' đã tồn tại")
                    },
                    "Tạo mới thất bại");

            var entity = new ChatLieu
            {
                TenChatLieu = dto.TenChatLieu,
                MoTa = dto.MoTa
            };

            var created = await _repo.AddAsync(entity);
            return BaseResponse<ChatLieuDto>.Success(MapEntityToDto(created), "Tạo mới chất liệu thành công");
        }

        // Cập nhật chất liệu
        public async Task<BaseResponse<ChatLieuDto>> UpdateAsync(int maChatLieu, ChatLieuDto dto)
        {
            var entity = await _repo.GetByIdAsync(maChatLieu);
            if (entity == null)
                return BaseResponse<ChatLieuDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaChatLieu", "Không tìm thấy chất liệu") },
                    "Cập nhật thất bại");

            // Kiểm tra duplicate tên khác record hiện tại
            var duplicate = await _repo.GetByNameAsync(dto.TenChatLieu);
            if (duplicate != null && duplicate.MaChatLieu != maChatLieu)
                return BaseResponse<ChatLieuDto>.Error(
                    new List<ErrorDetail>
                    {
                        new ErrorDetail(nameof(dto.TenChatLieu), $"Tên chất liệu '{dto.TenChatLieu}' đã tồn tại")
                    },
                    "Cập nhật thất bại");

            entity.TenChatLieu = dto.TenChatLieu;
            entity.MoTa = dto.MoTa;

            var updated = await _repo.UpdateAsync(entity);
            return BaseResponse<ChatLieuDto>.Success(MapEntityToDto(updated), "Cập nhật chất liệu thành công");
        }

        // Xóa chất liệu

        public async Task<BaseResponse<bool>> DeleteAsync(int maChatLieu)
        {
            var entity = await _repo.GetByIdAsync(maChatLieu);
            if (entity == null)
                return BaseResponse<bool>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaChatLieu", "Không tìm thấy chất liệu") },
                    "Xóa thất bại");

            var success = await _repo.DeleteAsync(maChatLieu);
            return BaseResponse<bool>.Success(success, success ? "Xóa chất liệu thành công" : "Xóa thất bại");
        }

        // Lấy chất liệu theo ID

        public async Task<BaseResponse<ChatLieuDto>> GetByIdAsync(int maChatLieu)
        {
            var entity = await _repo.GetByIdAsync(maChatLieu);
            if (entity == null)
                return BaseResponse<ChatLieuDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaChatLieu", "Không tìm thấy chất liệu") },
                    "Lấy dữ liệu thất bại");

            return BaseResponse<ChatLieuDto>.Success(MapEntityToDto(entity), "Lấy chất liệu thành công");
        }

        // Lấy tất cả chất liệu

        public async Task<BaseResponse<List<ChatLieuDto>>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            var dtos = entities.Select(MapEntityToDto).ToList();
            return BaseResponse<List<ChatLieuDto>>.Success(dtos, "Lấy danh sách chất liệu thành công");
        }

        // Mapping entity -> DTO

        private ChatLieuDto MapEntityToDto(ChatLieu entity)
        {
            return new ChatLieuDto
            {
                MaChatLieu = entity.MaChatLieu,
                TenChatLieu = entity.TenChatLieu,
                MoTa = entity.MoTa
            };
        }
    }
}