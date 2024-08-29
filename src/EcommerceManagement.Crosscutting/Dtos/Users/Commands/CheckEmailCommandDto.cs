using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceManagement.Crosscutting.Dtos.Users.Commands
{
    public class CheckEmailCommandDto
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Incorrect Email Format")]
        public string Email { get; set; }
    }
}
