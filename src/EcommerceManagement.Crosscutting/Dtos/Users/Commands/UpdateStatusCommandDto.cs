using EcommerceManagement.Crosscutting.Enums.Users;

namespace EcommerceManagement.Crosscutting.Dtos.Users.Commands
{
    public class UpdateStatusCommandDto
    {
        public string Email { get; set; }

        public UserStatus Status { get; set; }
    }
}
