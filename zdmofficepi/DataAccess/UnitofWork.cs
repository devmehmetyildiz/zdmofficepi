using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zdmofficepi.DataAccess.Repositories.Abstract;
using zdmofficepi.DataAccess.Repositories.Concrete;

namespace zdmofficepi.DataAccess
{
    public class UnitofWork : IUnitofWork
    {
        private ApplicationDBContext _applicationDBContext;
        public UnitofWork(ApplicationDBContext context)
        {
            _applicationDBContext = context;
            CategoriesRepository = new CategoriesRepository(_applicationDBContext);
            FileRepository = new FileRepository(_applicationDBContext);
            ProductgroupRepository = new ProductgroupRepository(_applicationDBContext);
            ProductRepositroy = new ProductRepository(_applicationDBContext);
            SubcategoriesRepositroy = new SubcategoriesRepository(_applicationDBContext);
            UserRepositroy = new UserRepository(_applicationDBContext);
        }

        public ICategoriesRepository CategoriesRepository { get; private set; }

        public IFileRepository FileRepository { get; private set; }

        public IProductgroupRepository ProductgroupRepository { get; private set; }

        public IProductRepositroy ProductRepositroy { get; private set; }

        public ISubcategoriesRepositroy SubcategoriesRepositroy { get; private set; }

        public IUserRepositroy UserRepositroy { get; private set; }

        public int Complate()
        {
            return _applicationDBContext.SaveChanges();
        }

        public void Dispose()
        {
            _applicationDBContext.Dispose();
        }
    }
}