using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace zdmofficepi.Models
{
    public class SubcategoryModel : BaseModel
    {
        public string Name { get; set; }
        public string Categoryuui { get; set; }

        [NotMapped]
        public CategoryModel Category { get; set; }
    }
}
