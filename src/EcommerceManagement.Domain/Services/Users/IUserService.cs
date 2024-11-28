using EcommerceManagement.Crosscutting.Dtos.Users.Commands;
using EcommerceManagement.Crosscutting.Dtos.Users;

namespace EcommerceManagement.Domain.Services.Users;

public interface IUserService
{
    public Task<UserDto> RegisterAsync(RegisterCommandDto createUserDto);

    public Task<UserLoginDto> LoginAsync(LoginCommandDto loginCommandDto);

    public Task<bool> CheckEmailAsync(CheckEmailCommandDto checkEmailCommandDto);

    public Task<bool> UpdatePasswordAsync(ForgetPasswordCommandDto forgetPasswordCommandDto);

    public Task<bool> UpdateStatusAsync(UpdateStatusCommandDto updateStatusCommandDto);

    public Task<List<string>> GetUserRolesAsync(Guid userId);
}