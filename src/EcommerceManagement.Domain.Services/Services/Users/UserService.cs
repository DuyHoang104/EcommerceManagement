using EcommerceManagement.Crosscutting.Dtos.Users;
using EcommerceManagement.Crosscutting.Dtos.Users.Commands;
using EcommerceManagement.Crosscutting.Enums.Users;
using EcommerceManagement.Domain.Entities.Addresses;
using EcommerceManagement.Domain.Entities.Users;
using EcommerceManagement.Domain.Repositories;
using EcommerceManagement.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EcommerceManagement.Domain.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto?> RegisterAsync(RegisterCommandDto createUserDto)
        {
            var userRepository = await _unitOfWork.GetRepositoryAsync<IUserRepository>();
            var query = userRepository.QueryHelper()
                                       .Where(x => x.UserName == createUserDto.UserName ||
                                                   x.Password == createUserDto.Password ||
                                                   x.Email == createUserDto.Email ||
                                                   x.Phone == createUserDto.Phone);

            var exists = await userRepository.ExistAsync(query);

            if (!exists)
            {
                var user = new User
                {
                    UserName = createUserDto.UserName,
                    Password = createUserDto.Password,
                    DateOfBirth = createUserDto.DateOfBirth,
                    Email = createUserDto.Email,
                    Phone = createUserDto.Phone,
                    LastAction = createUserDto.LastAction,
                    Status = UserStatus.InConfirm
                };

                foreach (var addressDetail in createUserDto.AddressDetails)
                {
                    var address = new Address { AddressDetails = addressDetail };
                    user.UserAddresses.Add(new UserAddress { User = user, Address = address });
                }

                await userRepository.InsertAsync(user);
                await _unitOfWork.SaveChangeAsync();

                var userDto = new UserDto
                {
                    ID = user.ID,
                    UserName = user.UserName,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    Phone = user.Phone,
                    LastAction = user.LastAction,
                    Addresses = createUserDto.AddressDetails
                };

                return userDto;
            }
            return null;
        }

        public async Task<UserLoginDto> LoginAsync(LoginCommandDto loginCommandDto)
        {
            var userRepository = await _unitOfWork.GetRepositoryAsync<IUserRepository>();
            var query = userRepository.QueryHelper().Where(x => x.UserName == loginCommandDto.UserName && x.Password == loginCommandDto.Password);
            var user = await userRepository.GetAsync(query);

            if (user == null || user.Status == UserStatus.Inactive)
            {
                throw new Exception("Your account is inactive. Please contact the administrator.");
            }

            return new UserLoginDto
            {
                ID = user.ID,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                DateOfBirth = user.DateOfBirth
            };
        }

        public async Task<bool> CheckEmailAsync(CheckEmailCommandDto checkEmailCommandDto)
        {
            var userRepository = await _unitOfWork.GetRepositoryAsync<IUserRepository>();
            var query = userRepository.QueryHelper().Where(x => x.Email == checkEmailCommandDto.Email);
            var userExists = await userRepository.ExistAsync(query);
            return userExists;
        }

        public async Task<bool> UpdatePasswordAsync(ForgetPasswordCommandDto forgetPasswordCommandDto)
        {
            var userRepository = await _unitOfWork.GetRepositoryAsync<IUserRepository>();
            var query = userRepository.QueryHelper().Where(x => x.Email == forgetPasswordCommandDto.Email);
            var userExists = await userRepository.ExistAsync(query);

            if (userExists)
            {
                var user = await userRepository.GetAsync(query);
                user.Password = forgetPasswordCommandDto.Password;
                await userRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateStatusAsync(UpdateStatusCommandDto updateStatusCommandDto)
        {
            var userRepository = await _unitOfWork.GetRepositoryAsync<IUserRepository>();
            var query = userRepository.QueryHelper().Where(x => x.Email == updateStatusCommandDto.Email);
            var userExists = await userRepository.ExistAsync(query);

            if (userExists)
            {
                var user = await userRepository.GetAsync(query);
                user.Status = updateStatusCommandDto.Status;
                await userRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            return false;
        }

        public async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            var userRepository = await _unitOfWork.GetRepositoryAsync<IUserRepository>();

            var userRoles = await userRepository.QueryHelper()
                                     .Include(u => u.UserAccounts)
                                     .Where(u => u.ID == userId)
                                     .SelectMany(u => u.UserAccounts.Select(ua => ua.Role.ToString()))
                                     .ToListAsync();
            return userRoles;
        }
    }
}