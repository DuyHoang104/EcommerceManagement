using EcommerceManagement.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceManagement.Domain.Repository
{
    public interface IGenericAuditRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>, IEntityAuditLog
    {
        public Task DeleteSoftAsync(TEntity entity);
    }
}
