using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceManagement.Crosscutting.Dtos.Users.Commands
{
    public class LoginCommandDto
    {
        public string UserName { get; set; }
     
        public string Password { get; set; }
    }
}
