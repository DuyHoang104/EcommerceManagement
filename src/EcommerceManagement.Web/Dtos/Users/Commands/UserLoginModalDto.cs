using System.ComponentModel.DataAnnotations;

namespace EcommerceManagement.Web.Dtos.Users.Commands
{
    public class UserLoginModalDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        [StringLength(40, ErrorMessage = "Username cannot exceed 40 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Username cannot contain spaces.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Password cannot contain spaces.")]
        public string Password { get; set; }
    }
}
