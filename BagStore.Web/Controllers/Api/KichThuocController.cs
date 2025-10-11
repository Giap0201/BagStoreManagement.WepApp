using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class KichThuocController : ControllerBase
    {
        private readonly IKichThuocRepository _repo;

        public KichThuocController(IKichThuocRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{maKichThuoc}")]
        public async Task<IActionResult> GetById(int maKichThuoc)
        {
            var result = await _repo.GetByIdAsync(maKichThuoc);
            if (result == null) return NotFound($"Khong tim thay kich thuoc co ma {maKichThuoc}");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KichThuocDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _repo.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { maKichThuoc = id }, request);
        }

        [HttpPut("{maKichThuoc}")]
        public async Task<IActionResult> Update(int maKichThuoc, [FromBody] KichThuocDto request)
        {
            if (maKichThuoc != request.MaKichThuoc)
            {
                return BadRequest("Ma kich thuoc khong khop");
            }
            var success = await _repo.UpdateAsync(request);
            if (!success) return NotFound($"Khong tim thay kich thuoc {maKichThuoc}");
            return NoContent();
        }

        [HttpDelete("{maKichThuoc}")]
        public async Task<IActionResult> Delete(int maKichThuoc)
        {
            var success = await _repo.DeleteAsync(maKichThuoc);
            if (!success) return NotFound($"Khong tim thay ma kich thuoc {maKichThuoc}");
            return NoContent();
        }
    }
}