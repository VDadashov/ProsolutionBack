using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using ProSolution.DAL.Contexts;
using ProSolution.DAL.Repositories.Common;

namespace ProSolution.DAL.Repositories
{
    public class PartnerRepository : GenericRepository<Partner>, IPartnerRepository
    {
        public PartnerRepository(AppDbContext context) : base(context)
        {
        }
    }
    
    
}
