using EcommerceManagement.Domain.Entities.Commons;
using EcommerceManagement.Domain.Repository;
using EcommerceManagement.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

namespace EcommerceManagement.Infrastructure.Repositories.Commons;

public class GenericAuditRepository<TEntity, TKey> : GenericRepository<TEntity, TKey>, IGenericAuditRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>, IEntityAuditLog
{
    public GenericAuditRepository(EcommerceManagementDBContext context) : base(context)
    {
    }

    public async Task DeleteSoftAsync(TEntity entity)
    {
        entity.LastAction = 'D';
        entity.LastActionBy = 0;
        entity.LastActionTimeStamp = DateTime.Now;

        _context.Set<TEntity>().Update(entity);
    }
}