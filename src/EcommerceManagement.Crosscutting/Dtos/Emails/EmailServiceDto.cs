using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceManagement.Crosscutting.Dtos.Emails
{
    public class EmailServiceDto
    {
        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

    }
}
