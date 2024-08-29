using EcommerceManagement.Domain.Entities.Users;
using EcommerceManagement.Domain.Entities.Commons;
using EcommerceManagement.Domain.Entities.Accounts;
using EcommerceManagement.Crosscutting.Enums.Users;

public class UserAccount : BaseEntity<Guid>, IEntityAuditLog
{
    public Guid UserID { get; set; }

    public User User { get; set; }

    public UserAccountRole Role { get; set; }

    public Guid AccountID { get; set; }

    public Account Account { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateTimeStamp { get; set; }

    public int LastActionBy { get; set; }

    public char LastAction { get; set; }

    public DateTime LastActionTimeStamp { get; set; }
}

