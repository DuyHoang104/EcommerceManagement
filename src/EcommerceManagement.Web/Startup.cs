using EcommerceManagement.Domain.Repositories;
using EcommerceManagement.Domain.Repository;
using EcommerceManagement.Domain.Services.Users;
using EcommerceManagement.Infrastructure.Databases;
using EcommerceManagement.Infrastructure.Repositories;
using EcommerceManagement.Web.BuilderAndServices;


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
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserService, UserService>();

        services.Configure<SmtpSettings>(Configuration.GetSection("Smtp"));
        services.AddTransient<IEmailSender, SmtpEmailSender>();
        services.AddTransient<IEmailHelper, EmailHelper>();

        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(1);
        });
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

        app.UseAuthorization();
        app.AddEndPointMVC();
            
        app.UseApplicationDatabase();
    }
}