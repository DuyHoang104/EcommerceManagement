using EcommerceManagement.Domain.Entities.Commons;
using EcommerceManagement.Domain.Entities.Users;

namespace EcommerceManagement.Domain.Entities
{
    public class UserAddress : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid AddressId { get; set; }
        public Address Address { get; set; }
    }
}
