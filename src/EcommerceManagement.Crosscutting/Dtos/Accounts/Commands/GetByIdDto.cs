namespace EcommerceManagement.Crosscutting.Dtos.Accounts.Commands
{
    public class GetByIdDto : GetAllDto
    {
        public Guid ID { get; set; }
    }
}