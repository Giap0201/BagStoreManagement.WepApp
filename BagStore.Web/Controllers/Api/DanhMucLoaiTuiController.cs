using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucLoaiTuiController : ControllerBase
    {
        private readonly IDanhMucLoaiTuiRepository _repo;

        public DanhMucLoaiTuiController(IDanhMucLoaiTuiRepository repo)
        {
            _repo = repo;
        }

        // GET: api/DanhMucLoaiTui
        // Trả về tất cả danh mục (HTTP 200 OK)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dtos = await _repo.GetAllDanhMucLoaiTuiAsync();
            return Ok(dtos);
        }

        // GET: api/DanhMucLoaiTui/{maLoaiTui}
        // Trả về một danh mục theo mã.
        [HttpGet("{maLoaiTui}")]
        public async Task<IActionResult> GetById(int maLoaiTui)
        {
            var dto = await _repo.GetDanhMucLoaiTuiByIdAsync(maLoaiTui);

            if (dto == null)
            {
                // Trả về 404 Not Found kèm thông báo rõ ràng khi tài nguyên không tồn tại.
                return NotFound($"Không tìm thấy Loại Túi với mã: {maLoaiTui}");
            }
            return Ok(dto);
        }

        // POST: api/DanhMucLoaiTui
        // Tạo mới danh mục.
        [HttpPost]
        public async Task<IActionResult> Create(DanhMucLoaiTuiDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var created = await _repo.CreateAsync(dto);

            // Trả về 201 Created và kèm theo URL để lấy tài nguyên mới.
            return CreatedAtAction(nameof(GetById), new { maLoaiTui = created.MaLoaiTui }, created);
        }

        // PUT: api/DanhMucLoaiTui/{maLoaiTui}
        // Cập nhật danh mục.
        [HttpPut("{maLoaiTui}")]
        public async Task<IActionResult> Update(int maLoaiTui, DanhMucLoaiTuiDto dto)
        {
            // Kiểm tra tính nhất quán (Client Error - 400 Bad Request).
            if (maLoaiTui != dto.MaLoaiTui)
            {
                return BadRequest("Mã loại túi trong URL và Body không khớp.");
            }

            // Validation (Data Annotations) tự động được kiểm tra tại đây.

            var updated = await _repo.UpdateAsync(dto);

            if (updated == null)
            {
                // Trả về 404 Not Found nếu đối tượng không tồn tại để cập nhật.
                return NotFound($"Không tìm thấy Loại Túi với mã: {maLoaiTui} để cập nhật.");
            }

            // Trả về 200 OK kèm dữ liệu đã cập nhật.
            return Ok(updated);
        }

        // DELETE: api/DanhMucLoaiTui/{maLoaiTui}
        // Xóa danh mục.
        [HttpDelete("{maLoaiTui}")]
        public async Task<IActionResult> Delete(int maLoaiTui)
        {
            var success = await _repo.DeleteAsync(maLoaiTui);

            if (!success)
            {
                // Trả về 404 Not Found nếu không tìm thấy đối tượng để xóa.
                return NotFound($"Không tìm thấy Loại Túi với mã: {maLoaiTui} để xóa.");
            }

            // Trả về 204 No Content - Mã chuẩn cho DELETE thành công không cần nội dung.
            return NoContent();
        }
    }
}