using EcommerceManagement.Domain.Entities.Commons;
using System;
using System.Collections.Generic;

namespace EcommerceManagement.Domain.Entities.Users
{
    public class Address : BaseEntity<Guid>
    {
        public Address()
        {
            UserAddresses = new HashSet<UserAddress>();
        }
        public string AddressDetails { get; set; }
        public ICollection<UserAddress> UserAddresses { get; set; }
    }
}
