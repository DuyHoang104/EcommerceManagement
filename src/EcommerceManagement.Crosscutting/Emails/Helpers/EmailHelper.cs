using EcommerceManagement.Crosscutting.Emails.Dtos;
using RazorLight;

namespace EcommerceManagement.Crosscutting.Emails.Helpers
{
    public class EmailHelper
    {
        private readonly RazorLightEngine _razorLightEngine;

        public EmailHelper()
        {
            _razorLightEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(AppContext.BaseDirectory, "Emails", "Templates"))
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> LoadEmailTemplateAsync(string templateName, string userName, string confirmationLink)
        {
            var model = new EmailModelDto
            {
                UserName = userName,
                ConfirmationLink = confirmationLink
            };
            string result = await _razorLightEngine.CompileRenderAsync(templateName, model);
            return result;
        }
    }
}