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
}
