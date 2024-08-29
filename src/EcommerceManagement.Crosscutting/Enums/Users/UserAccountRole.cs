using System.ComponentModel;

namespace EcommerceManagement.Crosscutting.Enums.Users
{
    public enum UserAccountRole
    {
        [Description("Admin")]
        Admin = 0,

        [Description("Manager")]
        Manager = 1,

        [Description("Employee")]
        Employee = 2
    }
}