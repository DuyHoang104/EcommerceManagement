using Microsoft.AspNetCore.Http;

namespace EcommerceManagement.Web.Helpers
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AccountException ex)
            {
                await HandleAccountExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleAccountExceptionAsync(HttpContext context, AccountException ex)
        {
            context.Session.SetString("ErrorMessage", ex.Message);
            context.Response.Redirect("/account/index");
            await Task.CompletedTask;
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Session.SetString("ErrorMessage", ex.Message);
            context.Response.Redirect(context.Request.Path);
            await Task.CompletedTask;
        }
    }
}