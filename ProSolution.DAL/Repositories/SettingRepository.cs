using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using ProSolution.Core.Repositories.Common;
using ProSolution.DAL.Contexts;
using ProSolution.DAL.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.DAL.Repositories
{
    public class SettingRepository : GenericRepository<Setting>, ISettingRepository
    {
        public SettingRepository(AppDbContext context) : base(context)
        {
        }
    }
}
