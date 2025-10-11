using BagStore.Web.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KichThuocController : Controller
    {
        private readonly HttpClient _httpClient;

        public KichThuocController(IHttpClientFactory httpClientFactory)
        {
            // Tạo HttpClient từ IHttpClientFactory
            _httpClient = httpClientFactory.CreateClient("BagStoreApi");
        }

        // Lấy danh sách kích thước
        public async Task<IActionResult> Index()
        {
            try
            {
                // Gọi API
                var response = await _httpClient.GetAsync("api/kichthuoc");

                // Nếu API trả lỗi thì trả về View rỗng
                if (!response.IsSuccessStatusCode)
                    return View(new List<KichThuocDto>());

                // Đọc JSON
                var json = await response.Content.ReadAsStringAsync();

                // Deserialize JSON → List<KichThuocDto>
                var data = JsonConvert.DeserializeObject<List<KichThuocDto>>(json);

                // Trả dữ liệu sang View
                return View(data);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi mạng/API
                Console.WriteLine("Lỗi khi gọi API: " + ex.Message);
                return View(new List<KichThuocDto>());
            }
        }
    }
}