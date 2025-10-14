using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepo;

    public CartController(ICartRepository cartRepo)
    {
        _cartRepo = cartRepo;
    }

    [HttpGet("getcart/{userId}")]
    public IActionResult GetCart(int userId)
    {

        var cartItems = _cartRepo.GetCartItems(userId);

        if (cartItems == null || cartItems.Items == null || !cartItems.Items.Any())
            return NotFound("Giỏ hàng trống");

        return Ok(cartItems);

    }
    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddCartItemRequest request)
    {
        if (request == null || request.SoLuong <= 0)
            return BadRequest("Dữ liệu không hợp lệ.");

        var result = await _cartRepo.AddSanPhamAsync(request);

        if (!result)
            return BadRequest("Không thể thêm sản phẩm vào giỏ hàng.");

        return Ok("Đã thêm sản phẩm vào giỏ hàng!");
    }

}
