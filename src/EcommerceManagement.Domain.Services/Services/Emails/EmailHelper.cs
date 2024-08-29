using EcommerceManagement.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

public class EmailHelper : IEmailHelper
{
    private readonly IWebHostEnvironment _env;

    public EmailHelper(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> LoadEmailTemplateAsync(string templatePath, string userName, string confirmationLink)
    {
        string filePath = Path.Combine(_env.ContentRootPath, "Views", "User", templatePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Template file not found.", filePath);
        }

        string templateContent = await File.ReadAllTextAsync(filePath);
        templateContent = templateContent.Replace("[UserName]", userName);
        templateContent = templateContent.Replace("[ConfirmationLink]", confirmationLink);

        return templateContent;
    }
}
