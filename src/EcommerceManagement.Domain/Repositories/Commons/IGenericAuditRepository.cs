using EcommerceManagement.Domain.Entities.Commons;

namespace EcommerceManagement.Domain.Repositories.Commons
{
    public interface IGenericAuditRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>, IEntityAuditLog
    {
        public Task DeleteSoftAsync(TEntity entity);
    }
}
