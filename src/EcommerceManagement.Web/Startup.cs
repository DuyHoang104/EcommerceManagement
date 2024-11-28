using EcommerceManagement.Crosscutting.Emails.Services;
using EcommerceManagement.Crosscutting.Emails.Settings;
using EcommerceManagement.Domain.Repositories;
using EcommerceManagement.Domain.Repositories.Commons;
using EcommerceManagement.Domain.Repository.Commons;
using EcommerceManagement.Domain.Services.Accounts;
using EcommerceManagement.Domain.Services.Services.Accounts;
using EcommerceManagement.Domain.Services.Users;
using EcommerceManagement.Infrastructure.Databases;
using EcommerceManagement.Infrastructure.Repositories;
using EcommerceManagement.Web.BuilderAndServices;
using EcommerceManagement.Web.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EcommerceManagement.Web;

public class Startup
{
    public IConfiguration Configuration { get; }
    public IHostEnvironment Environment { get; }

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    // Phương thức này được sử dụng để thêm các dịch vụ vào container DI.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMVCService();

        services.AddHttpContextAccessor();

        services.AddDatabaseModule(Configuration);

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUserAccountRepository, UserAccountRepository>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserService, UserService>();

        services.Configure<SmtpConfig>(Configuration.GetSection("Smtp"));
        services.AddTransient<IEmailSender, SmtpEmailSender>();
        services.AddScoped<IAccountService, AccountService>();

        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
        });

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.AccessDeniedPath = "/AccessDenied";
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("User", policy => policy.RequireRole("User"));
        });

        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
    }

    // Phương thức này được sử dụng để cấu hình HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSession();
        app.UseRouting();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
          {
              endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
          });

        app.AddEndPointMVC();

        app.UseApplicationDatabase();
    }
}