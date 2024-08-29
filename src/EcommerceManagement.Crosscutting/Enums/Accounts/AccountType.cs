using System.ComponentModel;

namespace EcommerceManagement.Crosscutting.Enums.Accounts
{
    public enum AccountType
    {
        [Description("Restaurant")]
        Restaurant = 0,

        [Description("Supply Chain")]
        SupplyChain = 1,

        [Description("Restaurant and Supply Chain")]
        RestaurantAndSupplyChain = 2
    }
}