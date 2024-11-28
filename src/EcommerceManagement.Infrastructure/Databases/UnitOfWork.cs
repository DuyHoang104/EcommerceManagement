using EcommerceManagement.Infrastructure.Databases;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceManagement.Infrastructure.Databases
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EcommerceManagementDBContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(EcommerceManagementDBContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<TRepository> GetRepositoryAsync<TRepository>() where TRepository : class
        {
            return _serviceProvider.GetService<TRepository>();
        }
    }
}
