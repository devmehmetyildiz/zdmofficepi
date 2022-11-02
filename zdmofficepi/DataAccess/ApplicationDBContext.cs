using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zdmofficepi.Models;

namespace zdmofficepi.DataAccess
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<ProductgroupModel> Productgroups { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<SubcategoryModel> Subcategories { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CompanyModel> Companies { get; set; }
    }
}
