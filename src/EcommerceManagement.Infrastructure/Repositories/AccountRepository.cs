using EcommerceManagement.Domain.Entities.Accounts;
using EcommerceManagement.Domain.Repositories.Commons;
using EcommerceManagement.Infrastructure.Databases;
using EcommerceManagement.Infrastructure.Repositories.Commons;

namespace EcommerceManagement.Infrastructure.Repositories
{
    public class AccountRepository : GenericAuditRepository<Account, Guid>, IAccountRepository
    {
        public AccountRepository(EcommerceManagementDBContext context) : base(context)
        {
        }
    }
}