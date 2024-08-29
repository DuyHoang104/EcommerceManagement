using EcommerceManagement.Crosscutting.Enums.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceManagement.Crosscutting.Dtos.Users.Commands
{
    public class UpdateStatusCommandDto
    {
        public string Email { get; set; }
    
        public UserStatus Status { get; set; }
    }
}
