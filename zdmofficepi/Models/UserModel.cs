using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zdmofficepi.Models
{
    public class UserModel : BaseModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
