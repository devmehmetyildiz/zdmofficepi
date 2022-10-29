using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zdmofficepi.DataAccess.Repositories.Abstract;
using zdmofficepi.Models;

namespace zdmofficepi.DataAccess.Repositories.Concrete
{
    public class SubcategoriesRepository : Repository<SubcategoryModel>, ISubcategoriesRepositroy
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<SubcategoryModel> _dbSet;
        public SubcategoriesRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<SubcategoryModel>();
        }
    }
}