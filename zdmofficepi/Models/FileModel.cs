using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace zdmofficepi.Models
{
    public class FileModel : BaseModel
    {
        public string Productuui { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string Filefolder { get; set; }
        public string Filepath { get; set; }
        public string Filetype { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
