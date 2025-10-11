using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatLieuController : ControllerBase
    {
        private readonly IChatLieuRepository _repo;

        public ChatLieuController(IChatLieuRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{maChatLieu}")]
        public async Task<IActionResult> GetById(int maChatLieu)
        {
            var result = await _repo.GetByIdAsync(maChatLieu);
            if (result == null) return NotFound($"Khong tim thay chat lieu co ma {maChatLieu}");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChatLieuDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _repo.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { maChatLieu = id }, request);
        }

        [HttpPut("{maChatLieu}")]
        public async Task<IActionResult> Update(int maChatLieu, [FromBody] ChatLieuDto request)
        {
            if (maChatLieu != request.MaChatLieu) return BadRequest("Ma chat lieu khong khop");
            var success = await _repo.UpdateAsync(request);
            if (!success) return NotFound($"Khong tim thay chat lieu co ma {maChatLieu}");
            return NoContent();
        }

        [HttpDelete("{maChatLieu}")]
        public async Task<IActionResult> Delete(int maChatLieu)
        {
            var success = await _repo.DeleteAsync(maChatLieu);
            if (!success) return NotFound($"Khong tim thay chat lieu co ma {maChatLieu}");
            return NoContent();
        }
    }
}