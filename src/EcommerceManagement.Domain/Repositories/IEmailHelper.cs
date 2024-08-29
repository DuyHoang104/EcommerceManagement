using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceManagement.Domain.Repositories
{
    public interface IEmailHelper
    {
        public Task<string> LoadEmailTemplateAsync(string templatePath, string userName, string confirmationLink);
    }
}
