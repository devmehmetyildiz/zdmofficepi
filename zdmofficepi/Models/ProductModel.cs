using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace zdmofficepi.Models
{
    public class ProductModel : BaseModel
    {
        public string Groupuui { get; set; }
        public string Name { get; set; }
        public string Productcode { get; set; }
        public string Dimension { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public bool IsFileChanged { get; set; }
        [NotMapped]
        public bool IsDataChanged { get; set; }
        [NotMapped]
        public ProductgroupModel Productgroup { get; set; }
     
    }
}
