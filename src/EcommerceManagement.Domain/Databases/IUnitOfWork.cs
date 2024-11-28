namespace EcommerceManagement.Infrastructure.Databases
{
    public interface IUnitOfWork : IDisposable
    {
        public Task SaveChangeAsync();

        public Task<TRepository> GetRepositoryAsync<TRepository>() where TRepository : class;
    }
}
