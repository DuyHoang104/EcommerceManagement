using AutoMapper;
using EcommerceManagement.Crosscutting.Dtos.Accounts;
using EcommerceManagement.Crosscutting.Dtos.Accounts.Commands;
using EcommerceManagement.Crosscutting.Dtos.Users;
using EcommerceManagement.Crosscutting.Dtos.Users.Commands;
using EcommerceManagement.Crosscutting.Enums.Users;
using EcommerceManagement.Web.Dtos.Accounts.Commands;
using EcommerceManagement.Web.Dtos.Users;
using EcommerceManagement.Web.Dtos.Users.Commands;
namespace EcommerceManagement.Web.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User
            CreateMap<RegisterModalDto, RegisterCommandDto>()
            .ForMember(dest => dest.LastAction, opt => opt.MapFrom(src => 'A'))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatus.InConfirm))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Value));

            CreateMap<UserDto, UserModalDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatus.InConfirm));

            CreateMap<UserLoginModalDto, LoginCommandDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<CheckEmailModalDto, CheckEmailCommandDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<ForgetPasswordModalDto, ForgetPasswordCommandDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            //Account
            CreateMap<AccountDto, GetModalDto>();
            CreateMap<CreateModalDto, SaveAccountDto>()
                .ForMember(dest => dest.UserID, opt => opt.Ignore());
            CreateMap<EditModalDto, EditAccountDto>();
            CreateMap<EditAccountDto, EditModalDto>();
            CreateMap<SaveModalDto, SaveAccountDto>();
        }
    }
}