using Microsoft.AspNetCore.Http;
using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Products.ProductReviews;

namespace ProSolution.BL.Services.Interfaces;

public interface IProductService 
{
    Task CreateAsync(ProductCreateDto dto);
    Task<ImageUploadResultDto> ImageUploadAsync(IFormFile file, bool isMain,string altText);
    Task UpdateAsync(string id, ProductUpdateDto dto);
    Task SoftDeleteAsync(string id, bool isDelete);
    Task DeleteAsync(string id);
    Task<ICollection<ProductGetDto>> GetAllAsync();
    Task<PaginationDto<ProductGetDto>> GetAllFilteredAsync(string? search, string? categoryId, int take, int page, 
        int order, bool isDeleted,double? minPrice,double? maxPrice, bool isDiscount, ICollection<string>? featureslugs);
    Task<ProductGetDto> GetByIdAsync(string id);
    Task<ProductGetDto> GetBySlugAsync(string slug);

    Task SoldCountUpdate(string id, SoldCountDto dto);

    //bunlari yazmag
    Task CreateReviewAsync(ProductReviewCreateDto dto);
    
    Task DeleteReviewAsync(string id);

    Task<string> DeleteImage(string path);



}
