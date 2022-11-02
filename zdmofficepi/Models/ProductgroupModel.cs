using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace zdmofficepi.Models
{
    public class ProductgroupModel :BaseModel
    {
        public ProductgroupModel()
        {
            Products = new List<ProductModel>();
        }
        public string Name { get; set; }
        [NotMapped]
        public List<ProductModel> Products { get; set; }
        public bool IsSet { get; set; }
        public double Price { get; set; }
        public string Categoryuuid { get; set; }
        public string Subcategoryuuid { get; set; }
        [NotMapped]
        public CategoryModel Category { get; set; }
        [NotMapped]
        public SubcategoryModel Subcategory { get; set; }
        public string Companyuuid { get; set; }
        [NotMapped]
        public CompanyModel Company { get; set; }
    }
}
