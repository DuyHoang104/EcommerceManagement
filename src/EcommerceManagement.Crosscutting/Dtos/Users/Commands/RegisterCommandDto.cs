using EcommerceManagement.Crosscutting.Enums.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceManagement.Crosscutting.Dtos.Users.Commands
{
    public class RegisterCommandDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public char LastAction { get; set; }

        public List<string> AddressDetails { get; set; } = new List<string>();

        public UserStatus Status { get; set; }
    }
}
