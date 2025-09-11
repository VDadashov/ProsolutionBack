using Microsoft.EntityFrameworkCore;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using ProSolution.DAL.Contexts;
using ProSolution.DAL.Repositories.Common;

namespace ProSolution.DAL.Repositories;

public class BasketRepository : GenericRepository<Basket>, IBasketRepository
{
    private readonly DbSet<BasketItem> _dbBasketItems;
    public BasketRepository(AppDbContext context) : base(context)
    {
        _dbBasketItems = context.Set<BasketItem>();
    }

    public void DeleteBasketItem(BasketItem basketItem)
    {
        _dbBasketItems.Remove(basketItem);
    }
}
