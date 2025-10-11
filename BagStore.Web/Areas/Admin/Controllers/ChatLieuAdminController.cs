using BagStore.Web.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChatLieuAdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public ChatLieuAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7013/api/"); // chú ý dấu '/'
        }

        // GET: /Admin/ChatLieuAdmin
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var list = await _httpClient.GetFromJsonAsync<List<ChatLieuDto>>("ChatLieu");
                return View(list ?? new List<ChatLieuDto>());
            }
            catch
            {
                ViewBag.ErrorMessage = "Không thể tải dữ liệu danh sách chất liệu.";
                return View(new List<ChatLieuDto>());
            }
        }

        // GET: /Admin/ChatLieuAdmin/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ChatLieuDto());
        }

        // POST: /Admin/ChatLieuAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChatLieuDto dto)
        {
            // 1) DataAnnotations check
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            // 2) Thêm kiểm tra thủ công nếu cần (ví dụ: trim, kiểm tra ký tự)
            dto.TenChatLieu = dto.TenChatLieu?.Trim();
            if (string.IsNullOrWhiteSpace(dto.TenChatLieu))
            {
                ModelState.AddModelError(nameof(dto.TenChatLieu), "Tên chất liệu không được chỉ chứa khoảng trắng.");
                return View(dto);
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("ChatLieu", dto);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Thêm chất liệu thành công.";
                    return RedirectToAction(nameof(Index));
                }

                // Nếu API trả BadRequest kèm nội dung lỗi, show cho người dùng
                var errContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Thêm thất bại: {response.ReasonPhrase}. {errContent}");
            }
            catch
            {
                ModelState.AddModelError("", "Lỗi kết nối tới dịch vụ. Vui lòng thử lại sau.");
            }

            return View(dto);
        }

        // GET: /Admin/ChatLieuAdmin/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var dto = await _httpClient.GetFromJsonAsync<ChatLieuDto>($"ChatLieu/{id}");
                if (dto == null) return RedirectToAction(nameof(Index));
                return View(dto);
            }
            catch
            {
                TempData["ErrorMessage"] = "Không thể tải dữ liệu để sửa.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Admin/ChatLieuAdmin/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ChatLieuDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            dto.TenChatLieu = dto.TenChatLieu?.Trim();
            if (string.IsNullOrWhiteSpace(dto.TenChatLieu))
            {
                ModelState.AddModelError(nameof(dto.TenChatLieu), "Tên chất liệu không được chỉ chứa khoảng trắng.");
                return View(dto);
            }

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"ChatLieu/{dto.MaChatLieu}", dto);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật thành công.";
                    return RedirectToAction(nameof(Index));
                }

                var err = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Cập nhật thất bại: {response.ReasonPhrase}. {err}");
            }
            catch
            {
                ModelState.AddModelError("", "Lỗi kết nối tới dịch vụ. Vui lòng thử lại sau.");
            }

            return View(dto);
        }

        // GET: /Admin/ChatLieuAdmin/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"ChatLieu/{id}");
                if (response.IsSuccessStatusCode)
                    TempData["SuccessMessage"] = "Xóa thành công.";
                else
                    TempData["ErrorMessage"] = "Xóa thất bại.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Lỗi kết nối tới dịch vụ.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}