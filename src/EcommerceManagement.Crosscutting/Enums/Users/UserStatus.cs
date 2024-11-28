using System.ComponentModel;
namespace EcommerceManagement.Crosscutting.Enums.Users
{
    public enum UserStatus
    {
        [Description("Pending")]
        InConfirm = 10,

        [Description("Active")]
        Active = 20,

        [Description("Inactive")]
        Inactive = 30
    }
}