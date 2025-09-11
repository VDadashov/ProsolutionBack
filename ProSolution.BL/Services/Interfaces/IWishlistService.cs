using ProSolution.BL.DTOs.User.ProSolution.BL.DTOs.Wishlist;

public interface IWishlistService
{
    Task<List<WishlistItemDTO>> GetAllAsync(string userId);
    Task AddAsync(string userId, string productId);
    Task RemoveAsync(string userId, string productId);
}
