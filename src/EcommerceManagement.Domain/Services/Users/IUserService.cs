using EcommerceManagement.Crosscutting.Dtos.Users;
using EcommerceManagement.Crosscutting.Dtos.Users.Commands;

namespace EcommerceManagement.Domain.Services.Users;

public interface IUserService
{
    public Task<UserDto> RegisterAsync(RegisterCommandDto createUserDto);

    public Task<UserLoginDto> LoginAsync(LoginCommandDto loginCommandDto);

    public Task<bool> CheckEmailAsync(CheckEmailCommandDto checkEmailCommandDto);

    public Task<bool> UpdatePasswordAsync(ForgetPasswordCommandDto forgetPasswordCommandDto);

    public Task<bool> UpdateStatusAsync(UpdateStatusCommandDto updateStatusCommandDto);
}