using EcommerceManagement.Crosscutting.Dtos.Accounts;
using EcommerceManagement.Crosscutting.Dtos.Accounts.Commands;

namespace EcommerceManagement.Domain.Services.Accounts
{
    public interface IAccountService
    {
        public Task<AccountDto> SaveAsync(SaveAccountDto saveAccountDto);

        public Task<List<AccountDto>> GetAllAsync(GetAllDto getAllDto);

        public Task<EditAccountDto> GetByIdAsync(GetByIdDto getByIdDto);

        public Task<bool> DeleteByIdAsync(GetByIdDto getByIdDto);
    }
}