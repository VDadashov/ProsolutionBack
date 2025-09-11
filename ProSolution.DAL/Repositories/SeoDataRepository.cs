using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using ProSolution.DAL.Contexts;
using ProSolution.DAL.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.DAL.Repositories
{
    public class SeoDataRepository : GenericRepository<SeoData>, ISeoDataRepository
    {
        public SeoDataRepository(AppDbContext context) : base(context)
        {
        }
    }
}
