using BagStore.Services;
using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BagStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
                return NotFound(new { message = "Giỏ hàng trống." });

            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddCartItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _cartService.AddToCartAsync(request);
            if (!result)
                return BadRequest(new { message = "Không thể thêm sản phẩm vào giỏ hàng." });

            return Ok(new { message = "Đã thêm sản phẩm vào giỏ hàng thành công." });
        }

        [HttpDelete("{MaKH:int}/{MaChiTietSP:int}")]
        public async Task<IActionResult> DeleteCart(int MaKH, int MaChiTietSP)
        {
            try
            {
                var result = await _cartService.RemoveCartItemAsync(MaKH, MaChiTietSP);
                if (result)
                    return Ok(new { message = "Đã xóa sản phẩm khỏi giỏ hàng thành công." });

                return NotFound(new { message = "Không tìm thấy sản phẩm trong giỏ hàng." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}