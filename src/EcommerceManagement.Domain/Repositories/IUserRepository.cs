using EcommerceManagement.Domain.Entities.Users;
using EcommerceManagement.Domain.Repositories.Commons;

namespace EcommerceManagement.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
    }
}
