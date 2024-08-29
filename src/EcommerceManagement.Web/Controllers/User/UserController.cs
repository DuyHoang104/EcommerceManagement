using EcommerceManagement.Crosscutting.Attribute;
using EcommerceManagement.Crosscutting.Dtos.Emails;
using EcommerceManagement.Crosscutting.Dtos.Users;
using EcommerceManagement.Crosscutting.Dtos.Users.Commands;
using EcommerceManagement.Crosscutting.Enums.Users;
using EcommerceManagement.Domain.Repositories;
using EcommerceManagement.Domain.Repository;
using EcommerceManagement.Domain.Services.Users;
using EcommerceManagement.Web.Dtos.Email;
using EcommerceManagement.Web.Dtos.Users;
using EcommerceManagement.Web.Dtos.Users.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EcommerceManagement.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userServices;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailHelper _emailHelper;

        public UserController(IUserService userServices, IUserRepository userRepository, IEmailSender emailSender, IWebHostEnvironment env, IEmailHelper emailHelper)
        {
            _userServices = userServices;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _env = env;
            _emailHelper = emailHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            RegisterModalDto createUserModalDto = new RegisterModalDto();
            return View("Register", createUserModalDto);
        }

        [HttpPost]
        [ValidateModel("Register")]
        public async Task<IActionResult> Register(RegisterModalDto createUserModalDto)
        {
            RegisterCommandDto registerCommandDto = new RegisterCommandDto
            {
                UserName = createUserModalDto.UserName,
                Password = createUserModalDto.Password,
                Email = createUserModalDto.Email,
                Phone = createUserModalDto.Phone,
                DateOfBirth = createUserModalDto.DateOfBirth.Value,
                LastAction = 'A',
                AddressDetails = createUserModalDto.AddressDetails,
                Status = UserStatus.InConfirm
            };

            try
            {
                UserDto userRegisterDto = await _userServices.RegisterAsync(registerCommandDto);

                UserModalDto userRegisterModalDto = new UserModalDto
                {
                    UserName = userRegisterDto.UserName,
                    Password = userRegisterDto.Password,
                    Email = userRegisterDto.Email,
                    AddressDetails = userRegisterDto.Addresses,
                    Phone = userRegisterDto.Phone,
                    DateOfBirth = userRegisterDto.DateOfBirth,
                    LastAction = userRegisterDto.LastAction,
                    Status = UserStatus.InConfirm
                };

                TempData["SuccessMessage"] = "Registration successful! You can now log in with your new account.";
                return RedirectToAction("EmailAuthenticate", new { email = userRegisterModalDto.Email, username = userRegisterModalDto.UserName });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "The user already exists with the information provided.";
                return View("Register", createUserModalDto);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            UserLoginModalDto loginUserModal = new UserLoginModalDto();
            return View("Login", loginUserModal);
        }

        [HttpPost]
        [ValidateModel("Login")]
        public async Task<IActionResult> Login(UserLoginModalDto loginUserModalDto)
        {
            try
            {
                LoginCommandDto loginUserDto = new LoginCommandDto
                {
                    UserName = loginUserModalDto.UserName,
                    Password = loginUserModalDto.Password
                };
                var userLoginDto = await _userServices.LoginAsync(loginUserDto);
                if (userLoginDto != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
            }
            return View("Login", loginUserModalDto);
        }

        [HttpGet]
        public IActionResult CheckEmail()
        {
            var checkEmailModalDto = new CheckEmailModalDto();
            return View("CheckEmail", checkEmailModalDto);
        }

        [HttpPost]
        [ValidateModel("CheckEmail")]
        public async Task<IActionResult> CheckEmail(CheckEmailModalDto checkEmailModalDto)
        {
            try
            {
                CheckEmailCommandDto checkEmailCommandDto = new CheckEmailCommandDto
                {
                    Email = checkEmailModalDto.Email
                };
                var emailExists = await _userServices.CheckEmailAsync(checkEmailCommandDto);
                if (emailExists)
                {
                    return RedirectToAction("ForgetPassword", new { email = checkEmailCommandDto.Email });
                }
                else
                {
                    ModelState.AddModelError("", "Email does not exist.");
                    return View("CheckEmail", checkEmailModalDto);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View("CheckEmail", checkEmailModalDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var forgetPasswordModalDto = new ForgetPasswordModalDto
            {
                Email = email
            };

            return View("ForgetPassword", forgetPasswordModalDto);
        }

        [HttpPost]
        [ValidateModel("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModalDto forgetPasswordModalDto)
        {
            try
            {
                ForgetPasswordCommandDto forgetPasswordCommandDto = new ForgetPasswordCommandDto
                {
                    Email = forgetPasswordModalDto.Email,
                    Password = forgetPasswordModalDto.Password
                };

                var result = await _userServices.UpdatePasswordAsync(forgetPasswordCommandDto);

                if (result)
                {
                    TempData["SuccessMessage"] = "Password has been updated successfully. Please log in with your new password.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update password.");
                    return View("ForgetPassword", forgetPasswordModalDto);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " Please try again!");
                return View("ForgetPassword", forgetPasswordModalDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EmailAuthenticate(string email, string username)
        {
            var emailDto = new EmailServiceDto
            {
                Subject = "Activate Your Account Now"
            };

            Guid emailGuid = Guid.NewGuid();
            string confirmationLink = Url.Action("ConfirmEmail", "User", new { Email = email, code = emailGuid }, Request.Scheme);

            try
            {
                emailDto.Message = await _emailHelper.LoadEmailTemplateAsync("EmailTemplate.cshtml", username, confirmationLink);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            emailDto.Email = email;

            await _emailSender.SendEmailAsync(emailDto);

            HttpContext.Session.SetString("EmailGuid", emailGuid.ToString());
            HttpContext.Session.SetString("EmailExpiry", DateTime.UtcNow.AddMinutes(5).ToString());

            return View("EmailAuthenticate");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, Guid code)
        {
            if (!HttpContext.Session.TryGetValue("EmailGuid", out var guidBytes) ||
                !Guid.TryParse(Encoding.UTF8.GetString(guidBytes), out var storedGuid) ||
                storedGuid != code)
            {
                TempData["Message"] = "Invalid or expired confirmation link.";
                return RedirectToAction("Login", "User");
            }

            if (!HttpContext.Session.TryGetValue("EmailExpiry", out var expiryTimeBytes))
            {
                TempData["Message"] = "Email confirmation session has expired or is invalid.";
                return RedirectToAction("Login", "User");
            }

            var expiryTime = DateTime.Parse(Encoding.UTF8.GetString(expiryTimeBytes));
            if (DateTime.UtcNow > expiryTime)
            {
                TempData["Message"] = "Email confirmation link has expired.";
                return RedirectToAction("Login", "User");
            }

            HttpContext.Session.Remove("EmailGuid");
            HttpContext.Session.Remove("EmailExpiry");

            try
            {
                var updateStatusCommandDto = new UpdateStatusCommandDto
                {
                    Email = email,
                    Status = UserStatus.Active
                };
                var statusUpdated = await _userServices.UpdateStatusAsync(updateStatusCommandDto);

                if (!statusUpdated)
                {
                    TempData["Message"] = "Failed to update user status.";
                    return RedirectToAction("Login", "User");
                }

                TempData["Message"] = "Email confirmation successful. You can now log in.";
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occurred while confirming email.";
                return RedirectToAction("Login", "User");
            }
        }

    }
}
