using EcommerceManagement.Crosscutting.Dtos.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceManagement.Domain.Repositories
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(EmailServiceDto emailDto);
    }
}
