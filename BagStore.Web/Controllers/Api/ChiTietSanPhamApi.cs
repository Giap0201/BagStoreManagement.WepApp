using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietSanPhamApi : ControllerBase
    {
        private readonly IChiTietSanPhamService _service;

        public ChiTietSanPhamApi(IChiTietSanPhamService service)
        {
            _service = service;
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateChiTietSanPham([FromBody])
    }
}