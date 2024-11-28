using EcommerceManagement.Crosscutting.Dtos.Accounts;
using EcommerceManagement.Crosscutting.Dtos.Accounts.Commands;
using EcommerceManagement.Domain.Repositories;
using EcommerceManagement.Domain.Services.Accounts;
using EcommerceManagement.Domain.Entities.Accounts;
using EcommerceManagement.Domain.Entities.Users;
using EcommerceManagement.Infrastructure.Databases;
using EcommerceManagement.Crosscutting.Enums.Users;
using EcommerceManagement.Domain.Repositories.Commons;

namespace EcommerceManagement.Domain.Services.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserRepository _userRepository;

        public AccountService(IUnitOfWork unitOfWork, IAccountRepository accountRepository, IUserAccountRepository userAccountRepository, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
            _userAccountRepository = userAccountRepository;
            _userRepository = userRepository;
        }

        private async Task<User> GetUserAsync(Guid userId)
        {
            var queryUser = _userRepository.QueryHelper().Where(x => x.ID == userId);
            var user = await _userRepository.GetAsync(queryUser) ?? throw new Exception("User not found.");
            return user;
        }

        public async Task<AccountDto> SaveAsync(SaveAccountDto saveAccountDto)
        {

            Account account = null;
            if (saveAccountDto.ID.HasValue)
            {
                var query = _accountRepository.QueryHelper().Where(x => x.ID == saveAccountDto.ID.Value);
                account = await _accountRepository.GetAsync(query) ?? throw new Exception($"Not found account.");
            }

            var queryUser = _userRepository.QueryHelper().Where(x => x.ID == saveAccountDto.UserID);
            var user = (await _userRepository.GetAsync(queryUser)) ?? throw new Exception("User not found.");
            if (user.Status != UserStatus.Active) throw new Exception($"User is not active. Cannot create account.");
            account ??= new Account();
            account.Name = saveAccountDto.Name;
            account.Address = saveAccountDto.Address;
            account.AccountType = saveAccountDto.AccountType;
            if (saveAccountDto.ID.HasValue)
            {
                await _accountRepository.UpdateAsync(account);
            }

            else
            {
                await _accountRepository.InsertAsync(account);
                var userAccount = new UserAccount
                {
                    User = user,
                    Account = account,
                    Description = "Admin for account",
                    Role = UserAccountRole.Admin
                };
                await _userAccountRepository.InsertAsync(userAccount);
            }

            await _unitOfWork.SaveChangeAsync();

            return new AccountDto
            {
                ID = account.ID,
                Name = account.Name,
                Address = account.Address,
                AccountType = account.AccountType
            };
        }

        public async Task<List<AccountDto>> GetAllAsync(GetAllDto getAllDto)
        {
            if (getAllDto.UserID == null) throw new Exception("UserID is required.");

            var user = await GetUserAsync(getAllDto.UserID);

            var queryable = _accountRepository.QueryHelper()
                .Where(account => account.UserAccounts.Any(ua => ua.UserID == getAllDto.UserID));

            var accounts = await _accountRepository.GetAllAsync(queryable);

            return accounts.Select(account => new AccountDto
            {
                ID = account.ID,
                Name = account.Name,
                Address = account.Address,
                AccountType = account.AccountType
            }).ToList();
        }

        public async Task<EditAccountDto> GetByIdAsync(GetByIdDto getByIdDto)
        {
            var user = await GetUserAsync(getByIdDto.UserID) ?? throw new Exception("User not found.");
            var queryAccount = _accountRepository.QueryHelper().Where(x => x.ID == getByIdDto.ID);
            var account = await _accountRepository.GetAsync(queryAccount) ?? throw new Exception($"Account not found.");
            return new EditAccountDto
            {
                ID = account.ID,
                Name = account.Name,
                Address = account.Address,
                AccountType = account.AccountType
            };
        }

        public async Task<bool> DeleteByIdAsync(GetByIdDto getByIdDto)
        {
            if (getByIdDto.UserID == null) throw new Exception("UserID is required.");

            var user = await GetUserAsync(getByIdDto.UserID);

            var queryAccount = _accountRepository.QueryHelper().Where(x => x.ID == getByIdDto.ID);
            var account = await _accountRepository.GetAsync(queryAccount) ?? throw new Exception($"Account not found.");
            await _accountRepository.DeleteAsync(account);
            await _unitOfWork.SaveChangeAsync();

            return true;
        }
    }
}