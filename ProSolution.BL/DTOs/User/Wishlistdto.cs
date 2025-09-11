namespace ProSolution.BL.DTOs.User
{
    namespace ProSolution.BL.DTOs.Wishlist
    {
        public class WishlistItemDTO
        {
            public string ProductId { get; set; } = null!;
            public string ProductTitle { get; set; } = null!;
            public double ProductPrice { get; set; }
            public string? ProductImageUrl { get; set; }
        }
       
        
            public class AddToWishlistDTO
            {
                public string ProductId { get; set; } = null!;
            }
        
      
        
            public class RemoveFromWishlistDTO
            {
                public string ProductId { get; set; } = null!;
            }
        


    }

}
