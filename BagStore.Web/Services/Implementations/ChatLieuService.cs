using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;
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

        public async Task<ChatLieuDto> CreateAsync(ChatLieuDto dto)
        {
            if (dto == null) throw new ArgumentException("Dữ liệu không hợp lệ");

            var entity = new ChatLieu
            {
                TenChatLieu = dto.TenChatLieu,
                MoTa = dto.MoTa
            };

            var result = await _repo.AddAsync(entity);

            return new ChatLieuDto
            {
                MaChatLieu = result.MaChatLieu,
                TenChatLieu = result.TenChatLieu,
                MoTa = result.MoTa
            };
        }

        public async Task<bool> DeleteAsync(int maChatLieu)
        {
            var entity = await _repo.GetByIdAsync(maChatLieu);
            if (entity == null) throw new ArgumentException("Mã chất liệu không tồn tại");

            return await _repo.DeleteAsync(maChatLieu);
        }

        public async Task<List<ChatLieuDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(e => new ChatLieuDto
            {
                MaChatLieu = e.MaChatLieu,
                TenChatLieu = e.TenChatLieu,
                MoTa = e.MoTa
            }).ToList();
        }

        public async Task<ChatLieuDto> GetByIdAsync(int maChatLieu)
        {
            var entity = await _repo.GetByIdAsync(maChatLieu);
            if (entity == null) throw new ArgumentException("Mã chất liệu không tồn tại");

            return new ChatLieuDto
            {
                MaChatLieu = entity.MaChatLieu,
                TenChatLieu = entity.TenChatLieu,
                MoTa = entity.MoTa
            };
        }

        public async Task<ChatLieuDto> UpdateAsync(int maChatLieu, ChatLieuDto dto)
        {
            var entity = await _repo.GetByIdAsync(maChatLieu);
            if (entity == null) throw new ArgumentException("Mã chất liệu không tồn tại");

            entity.TenChatLieu = dto.TenChatLieu;
            entity.MoTa = dto.MoTa;

            var result = await _repo.UpdateAsync(entity);

            return new ChatLieuDto
            {
                MaChatLieu = result.MaChatLieu,
                TenChatLieu = result.TenChatLieu,
                MoTa = result.MoTa
            };
        }
    }
}