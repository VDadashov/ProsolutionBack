using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using System.Linq.Expressions;

namespace ProSolution.BL.Services.Implements;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _repository;
    private readonly IMapper _mapper;

    public BasketService(
        IBasketRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<string> CreateAsync(BasketCreateDto dto)
    {
        // 1. ИСПОЛЬЗУЕМ AUTOMAPPER
        // Он создаст объект Basket и корректно заполнит его коллекцию BasketItems,
        // установив все необходимые связи между родительской и дочерними сущностями.
        var basket = _mapper.Map<Basket>(dto);

        // 2. Добавляем в репозиторий уже полностью собранный объект
        await _repository.AddAsync(basket);
        await _repository.SaveChangeAsync();

        return basket.Token!;
    }

    public async Task<string> UpdateAsync(string id, BasketUpdateDto dto)
    {
        Basket basket = await _getByIdAsync(id, includeItems: true);

        // Silinəcək itemlər
        if (dto.BasketItemIds != null && dto.BasketItemIds.Any())
        {
            var itemsToRemove = basket.BasketItems?
           .Where(x => dto.BasketItemIds.Contains(x.ProductId!))
           .ToList();

            if (itemsToRemove != null)
            {
                foreach (var item in itemsToRemove)
                    _repository.DeleteBasketItem(item);
            }
        }

        // Yeni itemlər əlavə olunur
        if (dto.BasketItemNews != null && dto.BasketItemNews.Any())
        {
            foreach (var newItem in dto.BasketItemNews)
            {
                basket.BasketItems?.Add(new BasketItem
                {
                    ProductId = newItem.ProductId,
                    Count = newItem.Count
                });
            }
        }

        _repository.Update(basket);
        await _repository.SaveChangeAsync();

        return basket.Token!;
    }



    public async Task<string> SoftDeleteAsync(string id)
    {
        Basket basket = await _getByIdAsync(id);
        basket.IsDeleted = !basket.IsDeleted;

        _repository.Update(basket);
        await _repository.SaveChangeAsync();

        return $"Basket \"{basket.Token}\" soft silindi.";
    }

    public async Task<string> DeleteAsync(string id)
    {
        Basket basket = await _getByIdAsync(id);
        _repository.Delete(basket);
        await _repository.SaveChangeAsync();

        return "Basket tamamilə silindi.";
    }

    public async Task<ICollection<BasketGetDto>> GetAllAsync()
    {
       var includes = new[] {    $"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Product)}",
            $"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Product)}.{nameof(Product.Images)}" };
        var baskets = await _repository
            .GetAll(false,includes)
            .ToListAsync();

        return _mapper.Map<ICollection<BasketGetDto>>(baskets);
    }

    public async Task<PaginationDto<BasketGetDto>> GetAllFilteredAsync(string? search, int take, 
        int page, int order,bool isDeleted)
    {
        if (page <= 0) throw new Exception("Invalid page number.");
        if (take <= 0) throw new Exception("Invalid take value.");
        if (order <= 0 || order > 2) throw new Exception("Invalid order value.");

        string[] includes = {
            $"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Product)}",
            $"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Product)}.{nameof(Product.Images)}"
        };

        // Общий фильтр
        Expression<Func<Basket, bool>> filter = x =>
            (string.IsNullOrEmpty(search) || x.Token!.ToLower().Contains(search.ToLower()));

        // Выбор ключа сортировки
        Expression<Func<Basket, object?>> orderBy = order switch
        {
            1 or 2 => x => x.Token,
            _ => throw new Exception("Order dəyəri düzgün deyil.")
        };

        bool orderByDesc = order is 2;

        // Кол-во всех продуктов
        double count = await _repository.CountAsync(filter, isDeleted);

        // Получение продуктов
        var items = await _repository
            .GetAllWhereByOrder(filter, orderBy, orderByDesc, isDeleted, (page - 1) * take, take, false, includes)
            .ToListAsync();

        var dtos = _mapper.Map<ICollection<BasketGetDto>>(items);

        return new PaginationDto<BasketGetDto>
        {
            Take = take,
            Search = search,
            Order = order,
            CurrentPage = page,
            Count = count,
            TotalPage = Math.Ceiling(count / take),
            Items = dtos,
        };
    }

    public async Task<BasketGetDto> GetByIdAsync(string id)
    {
        Basket basket = await _getByIdAsync(id, includeItems: true);
        return _mapper.Map<BasketGetDto>(basket);
    }

    public async Task<BasketGetDto> GetByTokenAsync(string token)
    {
       var includes = new[] {    $"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Product)}",
            $"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Product)}.{nameof(Product.Images)}" };
        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Token boş ola bilməz.");

        var basket = await _repository.GetByExpressionAsync(
            x => x.Token.Trim().ToLower().Contains(token.Trim().ToLower()), true, includes);
        basket.IsVerified=true; // Set IsVerified to true when basket is retrieved by token
        if (basket is null)
            throw new Exception($"Basket tapılmadı (token: {token})");

        _repository.Update(basket);
        await _repository.SaveChangeAsync(); // Ensure changes are saved
        return _mapper.Map<BasketGetDto>(basket);
    }









    //=================//
    // Private Methods //
    //=================//

    private async Task<Basket> _getByIdAsync(string id, bool includeItems = false)
    {
        if (string.IsNullOrEmpty(id))
            throw new Exception("ID boşdur.");

        string[]? includes = null;
        if (includeItems)
        {
            // Собираем инклуды только если это требуется
            includes = new[] {    $"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Product)}",
            $"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Product)}.{nameof(Product.Images)}" };
        }

        // Передаем массив includes (или null) в репозиторий
        var basket = await _repository.GetByIdAsync(id, true, includes);

        if (basket is null)
            throw new Exception($"Basket tapılmadı (id: {id})");

        return basket;
    }


}