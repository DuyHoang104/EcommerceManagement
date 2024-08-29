using EcommerceManagement.Domain.Entities.Users;
using EcommerceManagement.Domain.Repository;
using EcommerceManagement.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EcommerceManagement.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User, Guid>, IUserRepository
    {
        private readonly EcommerceManagementDBContext _context;

        public UserRepository(EcommerceManagementDBContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<User> QueryHelper()
        {
            return _context.Set<User>().AsQueryable();
        }
    }
}
