using EcommerceManagement.Domain.Entities.Users;
using EcommerceManagement.Domain.Repositories;
using EcommerceManagement.Infrastructure.Databases;
using EcommerceManagement.Infrastructure.Repositories.Commons;

namespace EcommerceManagement.Infrastructure.Repositories
{
    public class UserRepository : GenericAuditRepository<User, Guid>, IUserRepository
    {
        public UserRepository(EcommerceManagementDBContext context) : base(context)
        {
        }
    }
}
