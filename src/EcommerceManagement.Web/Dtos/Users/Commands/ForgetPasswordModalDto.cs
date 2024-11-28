using System.ComponentModel.DataAnnotations;

namespace EcommerceManagement.Web.Dtos.Users.Commands
{
    public class ForgetPasswordModalDto
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Incorrect Email Format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Password must not contain spaces.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
