using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace zdmofficepi.Models
{
    public class CategoryModel : BaseModel
    {
        public CategoryModel()
        {
            Subcategories = new List<SubcategoryModel>();
        }
        public string Name { get; set; }

        [NotMapped]
        public List<SubcategoryModel> Subcategories { get; set; }
    }
}
