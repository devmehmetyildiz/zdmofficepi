using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zdmofficepi.DataAccess.Repositories.Abstract;
using zdmofficepi.Models;

namespace zdmofficepi.DataAccess.Repositories.Concrete
{
    public class CompanyRepositroy : Repository<CompanyModel>, ICompanyRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<CompanyModel> _dbSet;
        public CompanyRepositroy(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<CompanyModel>();
        }
    }
}