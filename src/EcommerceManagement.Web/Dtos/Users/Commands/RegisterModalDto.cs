using System.ComponentModel.DataAnnotations;

namespace EcommerceManagement.Web.Dtos.Users.Commands
{
    public class RegisterModalDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "UserName")]
        [MaxLength(20, ErrorMessage = "Max length is 20 characters!")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Username must not contain spaces.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Password must not contain spaces.")]
        public string Password { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "Date Of Birth is required.")]
        [DateRange(100, 0)]
        [DateValidate(18, ErrorMessage = "You must be at least {0}.")]
        public DateOnly? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Incorrect Email Format.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        [MaxLength(24, ErrorMessage = "Max length is 24 characters!")]
        public List<string> AddressDetails { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [MaxLength(24, ErrorMessage = "Max length is 24 characters!")]
        [RegularExpression(@"^0[9875]\d{8}$", ErrorMessage = "Incorrect phone format.")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
    }
}
