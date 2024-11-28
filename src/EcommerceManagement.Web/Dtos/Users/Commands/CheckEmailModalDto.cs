using System.ComponentModel.DataAnnotations;

namespace EcommerceManagement.Web.Dtos.Users.Commands
{
    public class CheckEmailModalDto
    {
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Incorrect Email Format")]
        public string Email { get; set; }
    }
}
