using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zdmofficepi.DataAccess.Repositories.Abstract;
using zdmofficepi.Models;

namespace zdmofficepi.DataAccess.Repositories.Concrete
{
    public class ProductgroupRepository : Repository<ProductgroupModel>, IProductgroupRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<ProductgroupModel> _dbSet;
        public ProductgroupRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<ProductgroupModel>();
        }
    }
}