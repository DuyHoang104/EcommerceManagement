namespace EcommerceManagement.Crosscutting.Dtos.Accounts.Commands
{
    public class SaveAccountDto : AccountDto
    {
        public Guid UserID { get; set; }
    }
}