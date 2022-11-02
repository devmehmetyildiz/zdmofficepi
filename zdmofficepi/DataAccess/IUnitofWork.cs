using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zdmofficepi.DataAccess.Repositories.Abstract;

namespace zdmofficepi.DataAccess
{
    public interface IUnitofWork : IDisposable
    {
        ICategoriesRepository CategoriesRepository { get; }
        IFileRepository FileRepository { get; }
        IProductgroupRepository ProductgroupRepository { get; }
        IProductRepositroy ProductRepositroy { get; }
        ISubcategoriesRepositroy SubcategoriesRepositroy { get; }
        IUserRepositroy UserRepositroy { get; }
        ICompanyRepository CompanyRepository { get; }
        int Complate();
    }
}