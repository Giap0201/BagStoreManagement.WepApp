using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThuongHieuController : ControllerBase
    {
        private readonly IThuongHieuRepository _repo;

        public ThuongHieuController(IThuongHieuRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{maThuongHieu}")]
        public async Task<IActionResult> GetById(int maThuongHieu)
        {
            var result = await _repo.GetByIdAsync(maThuongHieu);
            if (result == null) return NotFound($"Khong tim thay thuong hieu co ma {maThuongHieu}");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ThuongHieuDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _repo.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { maThuongHieu = id }, request);
        }

        [HttpPut("{maThuongHieu}")]
        public async Task<IActionResult> Update(int maThuongHieu, [FromBody] ThuongHieuDto request)
        {
            if (maThuongHieu != request.MaThuongHieu) return BadRequest("Ma thuong hieu khong khop");
            var success = await _repo.UpdateAsync(request);
            if (!success) return NotFound($"Khong tim thay thuong hieu co ma {maThuongHieu}");
            return NoContent();
        }

        [HttpDelete("{maThuongHieu}")]
        public async Task<IActionResult> Delete(int maThuongHieu)
        {
            var success = await _repo.DeleteAsync(maThuongHieu);
            if (!success) return NotFound($"Khong tim thay thuong hieu co ma {maThuongHieu}");
            return NoContent();
        }
    }
}