using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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