using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Characteristics;
using ProSolution.BL.DTOs.FeatureOPtions;
using ProSolution.BL.DTOs.Products.ProductReviews;
using ProSolution.BL.Services.Interfaces;
using ProSolution.BL.Utilities.Helpers;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using ProSolution.DAL.Contexts;
using ProSolution.DAL.Repositories;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace ProSolution.BL.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly AppDbContext _context;
        private readonly IFeatureOptionRepository _featureOptionRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductFeatureKeysRepository _productFeatureKeysRepository;
        public ProductService(
            IProductRepository productRepository,
            ICloudStorageService cloudStorageService,
            IMapper mapper, ICategoryRepository categoryRepository,
            AppDbContext context, 
            IFeatureOptionRepository featureOptionRepository,
            IBrandRepository brandRepository,
            IProductFeatureKeysRepository productFeatureKeysRepository)
        {
            _productRepository = productRepository;
            _cloudStorageService = cloudStorageService;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _context = context;
            _featureOptionRepository = featureOptionRepository;
            _brandRepository = brandRepository;
            _productFeatureKeysRepository = productFeatureKeysRepository;
        }

        public async Task CreateAsync(ProductCreateDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            product.DetailSlug = _generateSlug(product.Title);
            
            
            var brandSlug = await _brandPath(product.BrandId);
            await _productRepository.AddProductSlugAsync(new ProductSlug
            {
                Slug = brandSlug,
                ProductId = product.Id
            });

            await _productRepository.AddAsync(product);

            if (dto.FeatureOptionItemIds != null)
            {
                foreach (var featureOptionItemId in dto.FeatureOptionItemIds)
                {
                    var productFeature = new ProductFeature
                    {
                        ProductId = product.Id,
                        FeatureOptionItemId = featureOptionItemId
                    };
                    await _productRepository.AddProductFeatureAsync(productFeature);

                    var path = await _featureOptionItemPath(featureOptionItemId);
                    var slug = _generateSlug(path); 

                    await _productRepository.AddProductSlugAsync(new ProductSlug
                    {
                        Slug = slug,
                        ProductId = product.Id,
                    });

                }
            }


            if (dto.CategoryItemIds != null)
            {
                foreach (var categoryId in dto.CategoryItemIds)
                {
                    await _productRepository.AddCategoryProductAsync(new CategoryProduct
                    {
                        CategoryId = categoryId,
                        ProductId = product.Id
                    });

                    var slug = await _categoryParth(categoryId);
                    await _productRepository.AddProductSlugAsync(new ProductSlug
                    {
                        Slug = slug,
                        ProductId = product.Id,
                    });
                }
            }

            await _productRepository.SaveChangeAsync();
        }



        public async Task<ImageUploadResultDto> ImageUploadAsync(IFormFile file, bool isMain,string altText)
        {
            string[] type =
            {
                "image/png",
                "image/jpg",
                "image/jpeg",
                "image/webp",
                "image/gif"
            };
            if (!file.ValidateTypeSize(10, type))
                throw new Exception("Sekil tipi ve ya olcusu uygun deyil");


            return new ImageUploadResultDto
            {
                Url = await _cloudStorageService.UploadFileAsync(file, "products"),
                IsMain = isMain,
                AltText = altText

            };
        }
        public async Task SoldCountUpdate(string id, SoldCountDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id, true);
            if (product == null)
                throw new Exception("Product not found");

            product.SoldCount = dto.SoldCount;
            _productRepository.Update(product);
            await _productRepository.SaveChangeAsync();

        }
        public async Task UpdateAsync(string id, ProductUpdateDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id, true,
                $"{nameof(Product.Images)}",
                $"{nameof(Product.ProductFeatures)}",
                $"{nameof(Product.CategoryProducts)}");

            if (product == null)
                throw new Exception("Product not found");

            _mapper.Map(dto, product);

            product.DetailSlug = _generateSlug(product.Title);

            // ===== ОБНОВЛЕНИЕ ИЗОБРАЖЕНИЙ =====
            if (dto.Images != null)
            {
                var incomingImagePaths = dto.Images
                    .Where(i => !string.IsNullOrWhiteSpace(i.ImagePath))
                    .Select(i => i.ImagePath)
                    .ToHashSet();

                var existingImages = product.Images?.ToList() ?? new List<ProductImage>();

                // Удаляем изображения, которых нет среди пришедших
                foreach (var oldImage in existingImages)
                {
                    if (!incomingImagePaths.Contains(oldImage.ImagePath))
                    {
                        await _cloudStorageService.DeleteFileAsync(oldImage.ImagePath);
                        await _productRepository.RemoveProductImageAsync(oldImage);
                    }
                }

                // Обновляем или добавляем изображения
                foreach (var dtoImage in dto.Images)
                {
                    var existingImage = existingImages.FirstOrDefault(i => i.ImagePath == dtoImage.ImagePath);
                    if (existingImage != null)
                    {
                        existingImage.AltText = dtoImage.AltText;
                        existingImage.IsMain = dtoImage.IsMain;
                    }
                    else
                    {
                        var newImage = new ProductImage
                        {
                            ProductId = product.Id,
                            ImagePath = dtoImage.ImagePath,
                            AltText = dtoImage.AltText,
                            IsMain = dtoImage.IsMain
                        };
                        await _productRepository.AddProductImageAsync(newImage);
                    }
                }
            }

            // ===== ОБНОВЛЕНИЕ SLUG'ОВ (удаляем все, добавим ниже по категориям и характеристикам) =====
            var oldSlugs = await _productRepository.GetProductSlugsAsync(id);
            await _productRepository.RemoveProductSlugsAsync(oldSlugs);

            if (dto.BrandId != null)
            {
                var brandSlug = await _brandPath(product.BrandId);
                await _productRepository.AddProductSlugAsync(new ProductSlug
                {
                    Slug = brandSlug,
                    ProductId = product.Id
                });

            }

            // ===== ОБНОВЛЕНИЕ КАТЕГОРИЙ =====
            if (dto.CategoryItemIds != null)
            {
                var newCategoryIds = dto.CategoryItemIds.ToHashSet();
                var existingCategoryIds = product.CategoryProducts?.Select(c => c.CategoryId).ToHashSet() ?? new();

                var toDelete = product.CategoryProducts!
                    .Where(c => !newCategoryIds.Contains(c.CategoryId))
                    .ToList();
                await _productRepository.RemoveCategoryProductsAsync(toDelete);

                var toAdd = newCategoryIds.Except(existingCategoryIds);
                foreach (var catId in toAdd)
                {
                    await _productRepository.AddCategoryProductAsync(new CategoryProduct
                    {
                        ProductId = product.Id,
                        CategoryId = catId
                    });

                    var path = await _categoryParth(catId);
                    var slug = _generateSlug(path);

                    await _productRepository.AddProductSlugAsync(new ProductSlug
                    {
                        Slug = slug,
                        ProductId = id
                    });
                }
            }

            // ===== ОБНОВЛЕНИЕ ХАРАКТЕРИСТИК (FEATURES) =====
            if (dto.FeatureOptionItemIds != null)
            {
                var newFeatureIds = dto.FeatureOptionItemIds.ToHashSet();
                var oldFeatureItems = await _productRepository.GetProductFeaturesAsync(product.Id);
                var existingFeatureIds = oldFeatureItems.Select(f => f.FeatureOptionItemId).ToHashSet();

                var toDelete = oldFeatureItems
                    .Where(f => !newFeatureIds.Contains(f.FeatureOptionItemId))
                    .ToList();
                await _productRepository.RemoveProductFeaturesAsync(toDelete);

                var toAdd = newFeatureIds.Except(existingFeatureIds);
                foreach (var featureId in toAdd)
                {
                    await _productRepository.AddProductFeatureAsync(new ProductFeature
                    {
                        ProductId = product.Id,
                        FeatureOptionItemId = featureId
                    });

                    var path = await _featureOptionItemPath(featureId);
                    var slug = _generateSlug(path);

                    await _productRepository.AddProductSlugAsync(new ProductSlug
                    {
                        Slug = slug,
                        ProductId = product.Id
                    });
                }
            }


            _productRepository.Update(product);
            await _productRepository.SaveChangeAsync();
        }

        // Остальные методы остаются без изменений...


        public async Task DeleteAsync(string id)
        {
          
            string[] includes =
            {
        $"{nameof(Product.Images)}",
        $"{nameof(Product.CategoryProducts)}.{nameof(CategoryProduct.Category)}",
        $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}"
    };

           
            var product = await _productRepository.GetByIdAsync(id, true, includes);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            
            if (product.Images != null)
            {
                foreach (var image in product.Images)
                {
                    try
                    {
                        await _cloudStorageService.DeleteFileAsync(image.ImagePath);
                    }
                    catch (Exception ex)
                    {
                        
                        throw new Exception($"Failed to delete image: {image.ImagePath}", ex);
                    }
                }
            }

           
            _productRepository.Delete(product);

            await _productRepository.SaveChangeAsync();
        }

        public async Task SoftDeleteAsync(string id, bool isDeleted)
        {
            var item = await _getByIdAsync(id);

            _productRepository.SoftDelete(item, isDeleted);
            await _productRepository.SaveChangeAsync();
        }

        public async Task<ICollection<ProductGetDto>> GetAllAsync()
        {
            string[] includes = {

                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}",

                    $"{nameof(Product.Images)}",
                    $"{nameof(Product.Brand)}",
                    $"{nameof(Product.CategoryProducts)}.{nameof(CategoryProduct.Category)}",

                     $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.Parent)}.{nameof(FeatureOptionItem.FeatureOption)}",

                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.FeatureOption)}",
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.Children)}",
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.Parent)}",
                    $"{nameof(Product.ProductSlugs)}",
                    $"{nameof(Product.ProductReviews)}"};


            ICollection<Product> items = await _productRepository.GetAll(false, includes).ToListAsync();

            ICollection<ProductGetDto> dtos = _mapper.Map<ICollection<ProductGetDto>>(items);
            foreach (var item in items)
            {
                dtos.FirstOrDefault(x => x.Id!.Contains(item.Id))!.RatingAvarage = await _productRepository.GetRatingAverageAsync(item.Id);
            }

            return dtos;
        }


        public async Task<PaginationDto<ProductGetDto>> GetAllFilteredAsync(
           string? search, string? slug, int take, int page, int order,
           bool isDeleted, double? minPrice, double? maxPrice, bool isDiscount, ICollection<string>? featureslugs)
        {
            if (page <= 0) throw new Exception("Invalid page number.");
            if (take <= 0) throw new Exception("Invalid take value.");
            if (order <= 0 || order > 14) throw new Exception("Invalid order value.");



            ICollection<FeatureOptionGetDto>? categorySpecificFeatures = null;
            if (!string.IsNullOrEmpty(slug))
            {
                var categorySlug = slug.Split('/').FirstOrDefault();

                if (!string.IsNullOrEmpty(categorySlug))
                {
                    var category = await _categoryRepository
                        .GetAllWhere(c => c.Slug.ToLower() == categorySlug.ToLower())
                        .FirstOrDefaultAsync();

                    if (category != null)
                    {
                        // ================== АВТОМАТИЗАЦИЯ ЗДЕСЬ ==================

                        // 1. Задаем желаемую глубину загрузки иерархии.
                        // Вы можете легко изменить это число. 5 уровней - более чем достаточно для большинства UI.
                        const int maxHierarchyDepth = 5;

                        // 2. Определяем базовый путь до первого уровня дочерних элементов.
                        var basePath = $"{nameof(ProductFeatureKeys.FeatureOptions)}.{nameof(FeatureOption.FeatureOptionItems)}";

                        // 3. Используем StringBuilder для эффективного построения строки в цикле.
                        var includeBuilder = new System.Text.StringBuilder(basePath);
                        for (int i = 0; i < maxHierarchyDepth; i++)
                        {
                            includeBuilder.Append($".{nameof(FeatureOptionItem.Children)}");
                        }
                        var fullHierarchyInclude = includeBuilder.ToString();

                        // 4. Используем сгенерированную строку в запросе.
                        var featureKeys = await _productFeatureKeysRepository
                            .GetAllWhere(
                                expression: fk => fk.CategoryId == category.Id,
                                includes: new[] { fullHierarchyInclude }
                            )
                            .FirstOrDefaultAsync();

                        // =======================================================

                        if (featureKeys?.FeatureOptions != null)
                        {
                            categorySpecificFeatures = _mapper.Map<ICollection<FeatureOptionGetDto>>(featureKeys.FeatureOptions);
                        }
                    }
                }
            }
    





            string[] includes = {
                
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}",
           
                    $"{nameof(Product.Images)}",
                    $"{nameof(Product.Brand)}",
                    $"{nameof(Product.CategoryProducts)}.{nameof(CategoryProduct.Category)}",

                     $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.Parent)}.{nameof(FeatureOptionItem.FeatureOption)}",

                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.FeatureOption)}",
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.Children)}",
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.Parent)}",
                    $"{nameof(Product.ProductSlugs)}",
                    $"{nameof(Product.ProductReviews)}"};

            ICollection<string> allSlugVariations = new List<string>();
            if (featureslugs != null && featureslugs.Any())
            {
                // 1. Normalize all incoming slugs by removing any leading slashes they might have.
                var normalizedSlugs = featureslugs.Select(s => s.TrimStart('/')).Distinct().ToList();
                // 2. Create the final list containing both the normalized version and the version with a leading slash.
                allSlugVariations = normalizedSlugs.Concat(normalizedSlugs.Select(s => "/" + s)).ToList();
            }
            // Общий фильтр
            Expression<Func<Product, bool>> filter = x =>
                (!isDiscount || x.DiscountPrice > 0) &&
                (string.IsNullOrEmpty(search) || x.Title.ToLower().Contains(search.ToLower())) &&
            (allSlugVariations.Count == 0 || x.ProductFeatures!.Any(a =>
            // Check if the feature's slug is in our prepared list
            allSlugVariations.Contains(a.FeatureOptionItem.Slug!) ||
            // OR check if the parent's slug is in our prepared list
            (a.FeatureOptionItem.Parent != null && allSlugVariations.Contains(a.FeatureOptionItem.Parent.Slug!))
        )) &&
                (!minPrice.HasValue || (x.DiscountPrice > 0 ? x.DiscountPrice >= minPrice : x.Price >= minPrice)) &&
                (!maxPrice.HasValue || (x.DiscountPrice > 0 ? x.DiscountPrice <= maxPrice : x.Price <= maxPrice)) &&
                (string.IsNullOrEmpty(slug) || x.ProductSlugs!.Any(a => a.Slug.ToLower().Contains(slug.ToLower())));


            // Дополнительный фильтр для скидок (если применимо)
            if (order is 11 or 12)
            {
                filter = filter.And(x => x.DiscountStartDate <= DateTime.UtcNow && x.DiscountEndDate >= DateTime.UtcNow);
            }

            // Выбор ключа сортировки
            Expression<Func<Product, object?>> orderBy = order switch
            {
                1 or 3 => x => x.Title,
                2 or 4 => x => x.CreatedAt,
                5 or 6 => x => x.Price,
                7 or 8 => x => x.RatingAvarage,
                9 or 10 => x => x.ViewCount,
                11 or 12 => x => x.DiscountStartDate,
                13 or 14 => x => x.SoldCount,
                _ => throw new Exception("Order dəyəri düzgün deyil.")
            };

            bool orderByDesc = order is 3 or 4 or 6 or 8 or 10 or 12 or 14;

            // Кол-во всех продуктов
            double count = await _productRepository.CountAsync(filter, isDeleted);

            // Получение продуктов
            var items = await _productRepository
                .GetAllWhereByOrder(filter, orderBy, orderByDesc, isDeleted, (page - 1) * take, take, false, includes)
                .ToListAsync();

            double? minAvailablePrice = await _productRepository.MinPriceAsync(search, slug, isDeleted);
            double? maxAvailablePrice = await _productRepository.MaxPriceAsync(search, slug, isDeleted);

            var dtos = _mapper.Map<ICollection<ProductGetDto>>(items);

            foreach (var item in items)
            {
                var dto = dtos.FirstOrDefault(x => x.Id!.Contains(item.Id));
                if (dto != null)
                {
                    dto.RatingAvarage = await _productRepository.GetRatingAverageAsync(item.Id);
                }
            }

            var usedFeatureIds = items
       .SelectMany(p => p.ProductFeatures ?? Enumerable.Empty<ProductFeature>())
       .Select(f => f.FeatureOptionItemId)
       .ToHashSet();    

            return new PaginationDto<ProductGetDto>
            {
                SlugPath = slug,
                Take = take,
                Search = search,
                Order = order,
                IsDiscount = isDiscount,
                CurrentPage = page,
                Count = count,
                FeatureIds = featureslugs, 
                TotalPage = Math.Ceiling(count / take),
                Items = dtos,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinAvailablePrice = minAvailablePrice,
                MaxAvailablePrice = maxAvailablePrice,
                CategoryFeatures = categorySpecificFeatures
            };
        }


        public async Task<ProductGetDto> GetByIdAsync(string id)
        {
            string[] includes = {
                    $"{nameof(Product.Images)}",
                    $"{nameof(Product.Brand)}",
                    $"{nameof(Product.CategoryProducts)}.{nameof(CategoryProduct.Category)}",
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.FeatureOption)}",
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.Children)}",
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.Parent)}",
                    $"{nameof(Product.ProductSlugs)}",
                    $"{nameof(Product.ProductReviews)}"};

            Product item = await _getByIdAsync(id, false, includes);

            // Только подтверждённые отзывы
            item.ProductReviews = await _productRepository
                .GetAllWhereProductReview(x =>
                    x.ProductId.Trim().ToLower() == id.Trim().ToLower() && x.Checked)
                .ToListAsync();

            item.ViewCount++;
            _productRepository.Update(item);
            await _productRepository.SaveChangeAsync();

            ProductGetDto dto = _mapper.Map<ProductGetDto>(item);
            dto.RatingAvarage = await _productRepository.GetRatingAverageAsync(item.Id);

            return dto;
        }

        public async Task<ProductGetDto> GetBySlugAsync(string slug)
        {
            string[] includes = {
                    $"{nameof(Product.Images)}",
                    $"{nameof(Product.Brand)}",
                    $"{nameof(Product.CategoryProducts)}.{nameof(CategoryProduct.Category)}",
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.FeatureOption)}",
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.Children)}",
                    $"{nameof(Product.ProductFeatures)}.{nameof(ProductFeature.FeatureOptionItem)}.{nameof(FeatureOptionItem.Parent)}",
                    $"{nameof(Product.ProductSlugs)}",
                    $"{nameof(Product.ProductReviews)}"};

            Product item = await _getBySlugAsync(slug, false, includes);
            
            // Только подтверждённые отзывы
            item.ProductReviews = await _productRepository
                .GetAllWhereProductReview(x =>
                    x.ProductId.Trim().ToLower() == item.Id.Trim().ToLower() && x.Checked)  
                .ToListAsync();

            item.ViewCount++;
            _productRepository.Update(item);
            await _productRepository.SaveChangeAsync();

            ProductGetDto dto = _mapper.Map<ProductGetDto>(item);
            dto.RatingAvarage = await _productRepository.GetRatingAverageAsync(item.Id);

            return dto;
        }

        public async Task CreateReviewAsync(ProductReviewCreateDto dto)
        {
            if (!await _productRepository.CheckUniqueAsync(x => x.Id.Trim().ToLower().Contains(dto.ProductId.Trim().ToLower())))
                throw new Exception("Product not found");

            var review = _mapper.Map<ProductReview>(dto);
            Product product = await _productRepository.GetByIdAsync(review.ProductId);
            if (product == null)
                throw new Exception("Product not found");
            await _productRepository.AddProductReview(review);
            await _productRepository.SaveChangeAsync();

            product.RatingAvarage = await _productRepository.GetRatingAverageAsync(product.Id);
            _productRepository.Update(product);
            await _productRepository.SaveChangeAsync();
        }

        public async Task DeleteReviewAsync(string id)
        {
            var review = await _context.Set<ProductReview>().FirstOrDefaultAsync(r => r.Id == id);
            if (review == null)
                throw new Exception("Review not found");

            await _productRepository.DeleteProductReview(id);
            await _productRepository.SaveChangeAsync();
        }
      


        public async Task<Category> GetMainCategoryBySlugAsync(string slug)
        {
            var category = await _categoryRepository
                .GetByExpressionAsync((x => x.Slug.Contains(slug)), false, $"{nameof(Category.Parent)}");

            while (category?.Parent != null)
            {
                category = category.Parent;
            }

            return category!;
        }

        public async Task<string> DeleteImage(string path)
        {
            await _cloudStorageService.DeleteFileAsync(path);
            return "Image deleted successfully";
        }


        private async Task<Product> _getByIdAsync(string id, bool isTracking = true, params string[] includes)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("The provided id is null or empty");
            Product item = await _productRepository.GetByIdAsync(id, isTracking, includes);
            if (item is null)
                throw new Exception($"Project not found({id})!");

            return item;
        }
        private async Task<Product> _getBySlugAsync(string slug, bool isTracking = true, params string[] includes)
        {
            if (string.IsNullOrEmpty(slug))
                throw new Exception("The provided id is null or empty");
            Product item = await _productRepository.GetByExpressionAsync(x => x.DetailSlug.ToLower().Trim().Contains(slug.ToLower().Trim()), isTracking, includes);
            if (item is null)
                throw new Exception($"Project not found({slug})!");

            return item;
        }


        private async Task<string> _brandPath(string brandId)
        {
            var brand = await _brandRepository.GetByIdAsync(brandId);
            if (brand == null) return string.Empty;

            return "brand/" + _generateSlug(brand.Title);
        }


        private async Task<string> _featureOptionItemPath(string? id)
        {
            if (string.IsNullOrWhiteSpace(id)) return string.Empty;

            var parts = new List<string>();

            // Загружаем FeatureOptionItem с родителем и опцией
            var item = await _featureOptionRepository.GetByIdFeatureOptionItemAsync(id, false,
                $"{nameof(FeatureOptionItem.Parent)}",
                $"{nameof(FeatureOptionItem.FeatureOption)}");

            if (item == null) return string.Empty;

            // Поднимаемся по дереву родительских опций
            while (item != null)
            {
                parts.Add(item.Name);

                if (string.IsNullOrWhiteSpace(item.ParentId)) break;

                item = await _featureOptionRepository.GetByIdFeatureOptionItemAsync(item.ParentId, false,
                    $"{nameof(FeatureOptionItem.Parent)}",
                    $"{nameof(FeatureOptionItem.FeatureOption)}");
            }

            // Добавляем саму FeatureOption в начало пути (например, "Videokart")
            if (item?.FeatureOption != null)
                parts.Add(item.FeatureOption.Name);

            parts.Reverse();
            return string.Join("/", parts.Select(_generateSlug));
        }




        private async Task<string> _categoryParth(string? id)
        {
            if (string.IsNullOrWhiteSpace(id)) return string.Empty;

            var parts = new List<string>();

            // İlk kateqoriyanı tapırıq (Parent məlumatları ilə birlikdə)
            var category = await _categoryRepository.GetByExpressionAsync(
                x => x.Id == id,
                includes: $"{nameof(Category.Parent)}"
            );

            if (category == null) return string.Empty;

            // Parent'lara qədər yuxarı çıxırıq
            while (category != null)
            {
                parts.Add(category.Title);
                if (string.IsNullOrWhiteSpace(category.ParentId)) break;

                category = await _categoryRepository.GetByExpressionAsync(
                    x => x.Id == category.ParentId,
                    includes: $"{nameof(Category.Parent)}"
                );
            }

            parts.Reverse();
            return string.Join("/", parts); // daha oxunaqlı format
        }
       

        private string _generateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;

            // Trim və bütün ardıcıl boşluqları tək boşluğa sal
            var normalized = System.Text.RegularExpressions.Regex.Replace(title.Trim().ToLower(), @"\s+", " ");

            // Sonra boşluqları "-" ilə əvəz et
            var slug = normalized.Replace(" ", "-");

            return slug.ToLowerInvariant();
        }




    }
}