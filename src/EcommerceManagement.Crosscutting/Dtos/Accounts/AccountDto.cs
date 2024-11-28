using EcommerceManagement.Crosscutting.Enums.Accounts;

namespace EcommerceManagement.Crosscutting.Dtos.Accounts
{
    public class AccountDto
    {
        public Guid? ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public AccountType AccountType { get; set; }
    }
}