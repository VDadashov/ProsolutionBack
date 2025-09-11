using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs.User.ProSolution.BL.DTOs.Wishlist;
using System.Security.Claims;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

      
        [HttpGet("{userId}")]
        public async Task<ActionResult<List<WishlistItemDTO>>> GetAllAsync(string userId)
        {
            var wishlist = await _wishlistService.GetAllAsync(userId);
            if (wishlist == null || wishlist.Count == 0)
            {
                return NotFound("No items found in the wishlist.");
            }
            return Ok(wishlist);
        }

     
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] AddToWishlistDTO addToWishlistDto)
        {
            if (addToWishlistDto == null || string.IsNullOrEmpty(addToWishlistDto.ProductId))
            {
                return BadRequest("Product ID is required.");
            }

           
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            await _wishlistService.AddAsync(userId, addToWishlistDto.ProductId);
            return Ok("Product added to wishlist.");
        }

     
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveAsync([FromBody] RemoveFromWishlistDTO removeFromWishlistDto)
        {
            if (removeFromWishlistDto == null || string.IsNullOrEmpty(removeFromWishlistDto.ProductId))
            {
                return BadRequest("Product ID is required.");
            }

            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            await _wishlistService.RemoveAsync(userId, removeFromWishlistDto.ProductId);
            return Ok("Product removed from wishlist.");
        }
    }
}
