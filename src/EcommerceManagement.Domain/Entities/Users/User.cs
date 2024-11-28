using EcommerceManagement.Crosscutting.Enums.Users;
using EcommerceManagement.Domain.Entities.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceManagement.Domain.Entities.Users
{
    [Table("Users")]
    public class User : BaseEntity<Guid>, IEntityAuditLog
    {
        public User()
        {
            UserAccounts = new HashSet<UserAccount>();
            UserAddresses = new HashSet<UserAddress>();
        }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public DateOnly DateOfBirth { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Phone]
        [MaxLength(15)]
        public string Phone { get; set; }

        public ICollection<UserAccount> UserAccounts { get; set; }

        public ICollection<UserAddress> UserAddresses { get; set; }

        public int LastActionBy { get; set; }

        public char LastAction { get; set; }

        public DateTime LastActionTimeStamp { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateTimeStamp { get; set; }

        public UserStatus Status { get; set; }
    }
}