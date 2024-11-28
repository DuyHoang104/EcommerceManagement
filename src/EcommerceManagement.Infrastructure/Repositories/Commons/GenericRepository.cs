using EcommerceManagement.Domain.Entities.Commons;
using EcommerceManagement.Domain.Repositories.Commons;
using EcommerceManagement.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

namespace EcommerceManagement.Domain.Repository.Commons
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected readonly EcommerceManagementDBContext _context;

        public GenericRepository(EcommerceManagementDBContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistAsync(IQueryable<TEntity> queryable)
        {
            return await queryable.AnyAsync();
        }

        public async Task<int> CountAsync(IQueryable<TEntity> queryable)
        {
            return await queryable.CountAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(IQueryable<TEntity> queryable)
        {
            return await queryable.ToListAsync();
        }

        public async Task<TEntity> GetAsync(IQueryable<TEntity> queryable)
        {
            return await queryable.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> QueryHelper()
        {
            return _context.Set<TEntity>().AsQueryable();
        }
    }
}
