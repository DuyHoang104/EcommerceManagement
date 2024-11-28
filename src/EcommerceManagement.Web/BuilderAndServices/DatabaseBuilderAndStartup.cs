using EcommerceManagement.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

namespace EcommerceManagement.Web.BuilderAndServices
{
    public static class DatabaseBuilderAndStartup
    {
        public static IServiceCollection AddDatabaseModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EcommerceManagementDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("EcommerceManagementDBConnection"));
                options.EnableDetailedErrors();
            });

            return services;
        }

        public static IApplicationBuilder UseApplicationDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EcommerceManagementDBContext>();
                dbContext.Database.OpenConnection();
                dbContext.Database.EnsureCreated();
            }

            return app;
        }

    }
}
