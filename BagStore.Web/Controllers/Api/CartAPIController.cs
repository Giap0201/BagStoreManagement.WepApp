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
        if (userId == null)
            return BadRequest("UserId không hợp lệ");

        var cartItems = _cartRepo.GetCartItems(userId);

        if (cartItems == null || !cartItems.Any())
            return NotFound("Giỏ hàng trống");

        return Ok(cartItems);
    }
}
