using ProSolution.Core.Entities;
using ProSolution.Core.Repositories.Common;

namespace ProSolution.Core.Repositories;

public interface IBasketRepository : IGenericRepository<Basket>
{
    void DeleteBasketItem(BasketItem basketItem);
}
