using EcommerceManagement.Domain.Entities.Users;
using EcommerceManagement.Domain.Repositories;
using EcommerceManagement.Infrastructure.Databases;
using EcommerceManagement.Infrastructure.Repositories.Commons;

namespace EcommerceManagement.Infrastructure.Repositories
{
    public class UserAccountRepository : GenericAuditRepository<UserAccount, Guid>, IUserAccountRepository
    {
        public UserAccountRepository(EcommerceManagementDBContext context) : base(context)
        {
        }
    }
}