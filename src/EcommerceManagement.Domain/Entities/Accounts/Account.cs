using EcommerceManagement.Crosscutting.Enums.Accounts;
using EcommerceManagement.Domain.Entities.Commons;
using EcommerceManagement.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceManagement.Domain.Entities.Accounts
{
    [Table("Accounts")]
    public class Account : BaseEntity<Guid>, IEntityAuditLog
    {
        public Account ()
        {
            UserAccounts = new HashSet<UserAccount>();
        }

        public string Name { get; set; }

        public string Address { get; set; }
        
        public AccountType AccountType { get; set; } 

        public ICollection<UserAccount> UserAccounts { get; set; }

        public int CreateBy  { get; set; }

        public DateTime CreateTimeStamp { get; set; }

        public int LastActionBy { get; set; }
        
        public char LastAction { get; set; }

        public DateTime LastActionTimeStamp { get; set; }
    }
}