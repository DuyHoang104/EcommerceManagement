using EcommerceManagement.Crosscutting.Enums.Users;

namespace EcommerceManagement.Web.Dtos.Users
{
    public class UserModalDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string Email { get; set; }

        public List<string> AddressDetails { get; set; }

        public string Phone { get; set; }

        public char LastAction { get; set; }

        public UserStatus Status { get; set; }

    }
}
