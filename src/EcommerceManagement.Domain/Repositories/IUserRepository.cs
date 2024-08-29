using EcommerceManagement.Domain.Entities.Commons;
using EcommerceManagement.Domain.Entities.Users;

namespace EcommerceManagement.Domain.Repository
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
    }
}
