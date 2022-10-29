using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace zdmofficepi.Models
{
    public class BaseModel 
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Createduser { get; set; }
        public string Updateduser { get; set; }
        public string Deleteuser { get; set; }
        public DateTime? Createdtime { get; set; }
        public DateTime? Updatetime { get; set; }
        public DateTime? Deletetime { get; set; }
        public bool IsActive { get; set; }
    }
}
