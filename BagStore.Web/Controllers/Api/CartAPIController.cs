using BagStore.Services;
using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BagStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
                return NotFound(new { message = "Bạn không có sản phẩm nào trong giỏ hàng !" });

            return Ok(cart);
        }

        [HttpPost("add")]
        [AllowAnonymous] // ✅ Cho phép Client thêm vào giỏ hàng
        public async Task<IActionResult> AddToCart([FromBody] AddCartItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _cartService.AddToCartAsync(request);
            if (!result)
                return BadRequest(new { message = "Không thể thêm sản phẩm vào giỏ hàng." });

            return Ok(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng thành công." });
        }

        [HttpDelete("{UserId}/{MaChiTietSP:int}")]
        [AllowAnonymous] // ✅ Cho phép Client xóa khỏi giỏ hàng
        public async Task<IActionResult> DeleteCart(string UserId, int MaChiTietSP)
        {
            try
            {
                var result = await _cartService.RemoveCartItemAsync(UserId, MaChiTietSP);
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