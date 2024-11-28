using EcommerceManagement.Domain.Entities.Accounts;
using EcommerceManagement.Domain.Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EcommerceManagement.Infrastructure.Databases
{
    public class EcommerceManagementDBContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EcommerceManagementDBContext(DbContextOptions<EcommerceManagementDBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }
        
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}