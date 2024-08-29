using EcommerceManagement.Crosscutting.Dtos.Users;
using EcommerceManagement.Crosscutting.Dtos.Users.Commands;
using EcommerceManagement.Domain.Entities;
using EcommerceManagement.Domain.Entities.Users;
using EcommerceManagement.Domain.Repository;
using EcommerceManagement.Infrastructure.Databases;

namespace EcommerceManagement.Domain.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UserDto> RegisterAsync(RegisterCommandDto createUserDto)
        {
            var userRepository = await _unitOfWork.GetRepositoryAsync<IUserRepository>();
            var query = userRepository.QueryHelper()
                                       .Where(x => x.UserName == createUserDto.UserName ||
                                                   x.Password == createUserDto.Password ||
                                                   x.Email == createUserDto.Email ||
                                                   x.Phone == createUserDto.Phone);

            var exists = await userRepository.ExistAsync(query);

            if (exists)
            {
                return null;
            }

            var user = new User
            {
                UserName = createUserDto.UserName,
                Password = createUserDto.Password,
                DateOfBirth = createUserDto.DateOfBirth,
                Email = createUserDto.Email,
                Phone = createUserDto.Phone,
                LastAction = createUserDto.LastAction,
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

        public async Task<UserLoginDto> LoginAsync(LoginCommandDto loginCommandDto)
        {
            var userRepository = await _unitOfWork.GetRepositoryAsync<IUserRepository>();
            var query = userRepository.QueryHelper().Where(x => x.UserName == loginCommandDto.UserName && x.Password == loginCommandDto.Password);
            var userExists = await userRepository.ExistAsync(query);

            if (userExists)
            {
                var userLoginDto = new UserLoginDto
                {
                    UserName = loginCommandDto.UserName,
                    Password = loginCommandDto.Password
                };
                return userLoginDto;
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }
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
    }
}