using EcommerceManagement.Domain.Entities.Commons;
using EcommerceManagement.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceManagement.Domain.Entities.Addresses
{
    [Table("Addresses")]
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
