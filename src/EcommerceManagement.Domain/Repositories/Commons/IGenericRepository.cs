using EcommerceManagement.Domain.Entities.Commons;

namespace EcommerceManagement.Domain.Repositories.Commons
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<int> CountAsync(IQueryable<TEntity> queryable);

        Task<bool> ExistAsync(IQueryable<TEntity> queryable);

        Task<IEnumerable<TEntity>> GetAllAsync(IQueryable<TEntity> queryable);

        Task<TEntity> GetAsync(IQueryable<TEntity> queryable);

        Task InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        IQueryable<TEntity> QueryHelper();
    }
}
