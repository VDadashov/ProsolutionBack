using AutoMapper;
using ProSolution.BL.DTOs.User.ProSolution.BL.DTOs.Wishlist;
using ProSolution.Core.Entities.Identity;
using ProSolution.Core.Repositories;

namespace ProSolution.BL.Services.Implements
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistIemRepostitory _wishlistRepository;
        private readonly IMapper _mapper;

        public WishlistService(IWishlistIemRepostitory wishlistRepository, IMapper mapper)
        {
            _wishlistRepository = wishlistRepository;
            _mapper = mapper;
        }

        public async Task<List<WishlistItemDTO>> GetAllAsync(string userId)
        {
            var wishlistItems = _wishlistRepository
                .GetAllWhere(w => w.UserId == userId, includes: "Product")
                .ToList();

            return _mapper.Map<List<WishlistItemDTO>>(wishlistItems);
        }

        public async Task AddAsync(string userId, string productId)
        {
            var exists = await _wishlistRepository.CheckUniqueAsync(w => w.UserId == userId && w.ProductId == productId);
            if (!exists)
            {
                var item = new WishlistItem
                {
                    UserId = userId,
                    ProductId = productId
                };
                await _wishlistRepository.AddAsync(item);
                await _wishlistRepository.SaveChangeAsync();
            }
        }

        public async Task RemoveAsync(string userId, string productId)
        {
            var item = await _wishlistRepository.GetByExpressionAsync(w => w.UserId == userId && w.ProductId == productId);
            if (item != null)
            {
                _wishlistRepository.Delete(item);
                await _wishlistRepository.SaveChangeAsync();
            }
        }
    }
}
