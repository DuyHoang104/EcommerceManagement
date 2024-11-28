using EcommerceManagement.Crosscutting.Dtos.Users.Commands;
using EcommerceManagement.Web.Dtos.Users.Commands;
using EcommerceManagement.Web.Dtos.Users;
using EcommerceManagement.Crosscutting.Attribute;
using EcommerceManagement.Crosscutting.Enums.Users;
using EcommerceManagement.Domain.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using EcommerceManagement.Crosscutting.Emails.Services;
using EcommerceManagement.Crosscutting.Emails.Dtos;
using EcommerceManagement.Crosscutting.Emails.Helpers;
using EcommerceManagement.Domain.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using EcommerceManagement.Web.Helpers;
using System.Security.Claims;
using AutoMapper;


namespace EcommerceManagement.Web.Controllers.User
{
    public class UserController : Controller
    {
        private readonly IUserService _userServices;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public UserController(IUserService userServices, IUserRepository userRepository,
        IEmailSender emailSender, IWebHostEnvironment env, IMapper mapper)
        {
            _userServices = userServices;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _env = env;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewBag.ErrorMessage = System.Text.Encoding.UTF8.GetString(errorMessage);
                HttpContext.Session.Remove("ErrorMessage");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (HttpContext.Session.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewBag.ErrorMessage = System.Text.Encoding.UTF8.GetString(errorMessage);
                HttpContext.Session.Remove("ErrorMessage");
            }
            return View("Register", new RegisterModalDto());
        }

        [HttpPost]
        [ValidateModel(nameof(Register))]
        public async Task<IActionResult> Register(RegisterModalDto createUserModalDto)
        {
            var userDto = await _userServices.RegisterAsync(_mapper.Map<RegisterCommandDto>(createUserModalDto))
                ?? throw new Exception("Failed to map register command.");
            var encryptedEmail = EncryptionHelper.Encrypt(_mapper.Map<UserModalDto>(userDto).Email);
            return RedirectToAction("EmailAuthenticate", new { email = encryptedEmail, username = _mapper.Map<UserModalDto>(userDto).UserName });
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (HttpContext.Session.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewBag.ErrorMessage = System.Text.Encoding.UTF8.GetString(errorMessage);
                HttpContext.Session.Remove("ErrorMessage");
            }
            UserLoginModalDto loginUserModal = new();
            return View("Login", loginUserModal);
        }

        [HttpPost]
        [ValidateModel("Login")]
        public async Task<IActionResult> Login(UserLoginModalDto loginUserModalDto)
        {
            var userLoginDto = await _userServices.LoginAsync(_mapper.Map<LoginCommandDto>(loginUserModalDto))
                ?? throw new Exception("Failed to map login command.");

            var userRoles = await _userServices.GetUserRolesAsync(userLoginDto.ID) ?? throw new Exception("Failed to get user roles.");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userLoginDto.ID.ToString()),
                new(ClaimTypes.Name, userLoginDto.UserName),
                new(ClaimTypes.Email, userLoginDto.Email),
                new(ClaimTypes.MobilePhone, userLoginDto.Phone),
                new(ClaimTypes.DateOfBirth, userLoginDto.DateOfBirth.ToString("dd/MM/yyyy"))
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmail()
        {
            if (HttpContext.Session.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewBag.ErrorMessage = System.Text.Encoding.UTF8.GetString(errorMessage);
                HttpContext.Session.Remove("ErrorMessage");
            }
            return View("CheckEmail", new CheckEmailModalDto());
        }

        [HttpPost]
        [ValidateModel("CheckEmail")]
        public async Task<IActionResult> CheckEmail(CheckEmailModalDto checkEmailModalDto)
        {
            var checkEmailCommandDto = _mapper.Map<CheckEmailCommandDto>(checkEmailModalDto) ?? throw new Exception("Invalid email.");
            var emailExists = await _userServices.CheckEmailAsync(checkEmailCommandDto);

            if (emailExists)
            {
                return RedirectToAction("ForgetPassword", new { email = checkEmailCommandDto.Email });
            }

            throw new Exception("Email does not exist.");
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (HttpContext.Session.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewBag.ErrorMessage = System.Text.Encoding.UTF8.GetString(errorMessage);
                HttpContext.Session.Remove("ErrorMessage");
            }
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
            var forgetPasswordCommandDto = _mapper.Map<ForgetPasswordCommandDto>(forgetPasswordModalDto) ?? throw new Exception("Failed to map forget password command.");

            var result = await _userServices.UpdatePasswordAsync(forgetPasswordCommandDto);

            if (result)
            {
                return RedirectToAction("Login");
            }

            throw new Exception("Failed to update password.");
        }

        [HttpGet]
        public async Task<IActionResult> EmailAuthenticate(string email, string username)
        {
            var decryptedEmail = EncryptionHelper.Decrypt(email) ?? throw new Exception("Failed to decrypt email.");

            var emailDto = new EmailDto
            {
                Subject = "Activate Your Account Now",
                To = decryptedEmail
            };

            Guid emailGuid = Guid.NewGuid();

            EmailHelper _emailHelper = new();
            emailDto.Message = await _emailHelper.LoadEmailTemplateAsync(
                "EmailTemplate", username, Url.Action("ConfirmEmail", "User", new { Email = email, code = emailGuid }, Request.Scheme));

            await _emailSender.SendEmailAsync(emailDto);

            HttpContext.Session.SetString("EmailGuid", emailGuid.ToString());
            HttpContext.Session.SetString("EmailExpiry", DateTime.UtcNow.AddMinutes(30).ToString());

            return View("EmailAuthenticate");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, Guid code)
        {
            var decryptedEmail = EncryptionHelper.Decrypt(email);
            if (!HttpContext.Session.TryGetValue("EmailExpiry", out var expiryTimeBytes) ||
                DateTime.UtcNow > DateTime.Parse(Encoding.UTF8.GetString(expiryTimeBytes)))
            {
                throw new Exception("Email confirmation link has expired.");
            }

            HttpContext.Session.Remove("EmailGuid");
            HttpContext.Session.Remove("EmailExpiry");

            var updateStatusCommandDto = new UpdateStatusCommandDto
            {
                Email = decryptedEmail,
                Status = UserStatus.Active
            };

            var statusUpdated = await _userServices.UpdateStatusAsync(updateStatusCommandDto);

            if (!statusUpdated)
            {
                throw new Exception("Failed to update user status.");
            }
            TempData["Message"] = "Email confirmation successful. You can now log in.";
            return View("Login");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            return View("Profile");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}