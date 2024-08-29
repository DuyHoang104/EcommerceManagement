using EcommerceManagement.Crosscutting.Enums.Users;

namespace EcommerceManagement.Web.Dtos.Users.Commands
{
    public class UpdateStatusModalDto
    {
        public string Email { get; set; }

        public UserStatus Status { get; set; }
    }
}
