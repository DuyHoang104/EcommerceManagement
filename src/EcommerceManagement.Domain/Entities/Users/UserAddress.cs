using EcommerceManagement.Domain.Entities.Addresses;
using EcommerceManagement.Domain.Entities.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceManagement.Domain.Entities.Users
{
    [Table("UserAddresses")]
    public class UserAddress : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid AddressId { get; set; }
        public Address Address { get; set; }
    }
}
