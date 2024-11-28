using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceManagement.Crosscutting.Dtos.Users
{
    public class UserLoginDto
    {
        public Guid ID { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public DateOnly DateOfBirth { get; set; }
    }
}
