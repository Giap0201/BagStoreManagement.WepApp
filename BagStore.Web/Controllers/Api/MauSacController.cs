using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MauSacController : ControllerBase
    {
        private readonly IMauSacRepository _repo;

        public MauSacController(IMauSacRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{maMauSac}")]
        public async Task<IActionResult> GetById(int maMauSac)
        {
            var result = await _repo.GetByIdAsync(maMauSac);
            if (result == null) return NotFound($"Khong tim thay mau sac co ma {maMauSac}");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MauSacDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _repo.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { maMauSac = id }, request);
        }

        [HttpPut("{maMauSac}")]
        public async Task<IActionResult> Update(int maMauSac, [FromBody] MauSacDto request)
        {
            if (maMauSac != request.MaMauSac) return BadRequest("Ma mau sac khong khop");
            var success = await _repo.UpdateAsync(request);
            if (!success) return NotFound($"Khong tim thay mau sac co ma {maMauSac}");
            return NoContent();
        }

        [HttpDelete("{maMauSac}")]
        public async Task<IActionResult> Delete(int maMauSac)
        {
            var success = await _repo.DeleteAsync(maMauSac);
            if (!success) return NotFound($"Khong tim thay mau sac co ma {maMauSac}");
            return NoContent();
        }
    }
}