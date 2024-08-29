namespace EcommerceManagement.Web.BuilderAndServices;

public static class MVCBuilderAndStartup
{
    public static IServiceCollection AddMVCService(this IServiceCollection services)
    {
        services.AddControllersWithViews().AddTagHelpersAsServices();
        services.AddRouting(options => options.LowercaseUrls = true);

        return services;
    }

    public static IApplicationBuilder AddEndPointMVC(this IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        return app;
    }
}