using EcommerceManagement.Crosscutting.Attribute;
using EcommerceManagement.Crosscutting.Enums.Accounts;
using EcommerceManagement.Web.Dtos.Accounts.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EcommerceManagement.Domain.Services.Accounts;
using System.Security.Claims;
using EcommerceManagement.Crosscutting.Dtos.Accounts.Commands;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using EcommerceManagement.Web.Helpers;

namespace EcommerceManagement.Web.Controllers.Account
{
    [Authorize]
    public class AccountController(IAccountService accountService, ILogger<AccountController> logger, IMapper mapper) : Controller
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ILogger<AccountController> _logger = logger;
        private readonly IMapper _mapper = mapper;

        private static SelectList GetAccountTypeSelectList()
        {
            return new SelectList(
                Enum.GetValues(typeof(AccountType))
                    .Cast<AccountType>()
                    .Select(e => new
                    {
                        Value = (int)e,
                        Text = e.GetEnumDescription()
                    }),
                "Value", "Text");
        }

        private Guid? GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new AccountException("User ID not found.");
            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                return userId;
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewBag.ErrorMessage = System.Text.Encoding.UTF8.GetString(errorMessage);
                HttpContext.Session.Remove("ErrorMessage");
            }

            ViewData["Type"] = GetAccountTypeSelectList();

            var userId = GetUserId();

            if (!userId.HasValue)
            {
                ViewBag.ErrorMessage = "Invalid user ID.";
                return View(new List<GetModalDto>());
            }

            var accountDtos = await _accountService.GetAllAsync(new GetAllDto { UserID = userId.Value });
            var accountDtoList = _mapper.Map<List<GetModalDto>>(accountDtos) ?? throw new AccountException("Failed to map accounts.");
            return View("Index", accountDtoList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewBag.ErrorMessage = System.Text.Encoding.UTF8.GetString(errorMessage);
                HttpContext.Session.Remove("ErrorMessage");
            }

            ViewData["Type"] = GetAccountTypeSelectList();
            return PartialView("Create", new CreateModalDto());
        }

        [HttpPost]
        [ValidateModel(nameof(Create))]
        public async Task<IActionResult> Create(CreateModalDto createModalDto)
        {
            var userId = GetUserId();

            if (userId.HasValue)
            {
                SaveAccountDto saveAccountDto = _mapper.Map<SaveAccountDto>(createModalDto) ?? throw new AccountException("Failed to map account.");
                saveAccountDto.UserID = userId.Value;

                var accountDto = await _accountService.SaveAsync(saveAccountDto);
                if (accountDto != null)
                {
                    var existingClaims = User.Claims.ToList() ?? throw new AccountException("Failed to get claims.");

                    var claims = new List<Claim>(existingClaims)
                    {
                        new(ClaimTypes.Role, "Admin")
                    };

                    var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                    await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
                }

                return RedirectToAction("Index");
            }

            throw new AccountException("Invalid user ID.");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (HttpContext.Session.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewBag.ErrorMessage = System.Text.Encoding.UTF8.GetString(errorMessage);
                HttpContext.Session.Remove("ErrorMessage");
            }

            ViewData["Type"] = GetAccountTypeSelectList();

            var userId = GetUserId();

            if (userId.HasValue)
            {
                var getByIdDto = new GetByIdDto
                {
                    UserID = userId.Value,
                    ID = id
                };

                var accountDto = await _accountService.GetByIdAsync(getByIdDto);
                var editModalDto = _mapper.Map<EditModalDto>(accountDto) ?? throw new AccountException("Failed to map account.");
                return RedirectToAction("Edit", editModalDto);
            }

            throw new AccountException("Invalid user ID.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateModel(nameof(Edit))]
        public async Task<IActionResult> Edit(SaveModalDto saveModalDto)
        {
            var userId = GetUserId();

            if (!User.IsInRole("Admin"))
            {
                throw new AccountException("You do not have permission to create accounts.");
            }

            if (userId.HasValue)
            {
                SaveAccountDto saveAccountDto = _mapper.Map<SaveAccountDto>(saveModalDto) ?? throw new Exception("Failed to map account.");
                saveAccountDto.UserID = userId.Value;
                var accountDto = await _accountService.SaveAsync(saveAccountDto);
                return RedirectToAction("Index");
            }

            throw new AccountException("Invalid user ID.");

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateModel(nameof(Delete))]
        public async Task<IActionResult> Delete(Guid ID)
        {
            var userId = GetUserId();

            if (!User.IsInRole("Admin"))
            {
                throw new AccountException("You do not have permission to create accounts.");
            }

            if (userId.HasValue)
            {
                var getByIdDto = new GetByIdDto
                {
                    UserID = userId.Value,
                    ID = ID
                };

                var accountDto = await _accountService.DeleteByIdAsync(getByIdDto);

                return RedirectToAction("Index");
            }

            throw new AccountException("Invalid user ID.");
        }
    }
}