using EcommerceManagement.Domain.Entities.Users;
using EcommerceManagement.Domain.Repositories.Commons;

namespace EcommerceManagement.Domain.Repositories
{
    public interface IUserAccountRepository : IGenericRepository<UserAccount, Guid>
    {

    }
}