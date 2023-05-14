using DAL.Entities;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;
using FFA6sem.Model.Services;
using FFA6sem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FFA6sem.Controllers
{
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDbCrud _dbCrud;
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IDbCrud dbCrud, IUserService userService, ILogger<AccountController> logger)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
            _dbCrud        = dbCrud;
            _userService   = userService;
            _logger        = logger;
        }

        [HttpPost]
        [Route("api/account/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel newUser = new UserModel()
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Role = model.Role,
                };

                // Добавление нового пользователя
                var result = await _dbCrud.CreateUser(newUser, _userManager);

                if (result.Succeeded)
                {
                    var helpUserId = await _userService.GetUserByName(newUser.UserName, _userManager);
                    string userId = helpUserId.Id;

                    // Установка куки
                    await _userService.SignInAsync(newUser, _signInManager);
                    return Ok(new { message = "Добавлен новый пользователь: " + newUser.UserName + " c ролью " + newUser.Role, userId = userId });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    var errorMsg = new
                    {
                        message = "Пользователь не добавлен",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };

                    return Created("", errorMsg);
                }
            }
            else
            {
                var errorMsg = new
                {
                    message = "Неверные входные данные",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };

                return Created("", errorMsg);
            }
        }

        [HttpPost]
        [Route("api/account/login")]
        //[AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel currentUserModel = new UserModel
                {
                    UserName = model.UserName,
                    Password = model.Password,                  
                };

                var result = await _userService.PasswordSignInAsync(currentUserModel, model.RememberMe, _signInManager);
                if (result.Succeeded)
                {
                    string? userRole = await _userService.GetUserRole(currentUserModel.UserName, _userManager);

                    var helpUserId = await _userService.GetUserByName(currentUserModel.UserName, _userManager);
                    string userId = helpUserId.Id;

                    _logger.LogInformation("Login {0} succeeded", currentUserModel.UserName);
                    return Ok(new { message = "Выполнен вход", userId = userId, userName = model.UserName, userRole = userRole });
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    var errorMsg = new
                    {
                        message = "Вход не выполнен",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };
                    _logger.LogInformation("Login {0} failed", currentUserModel.UserName);
                    return Created("", errorMsg);
                }
            }
            else
            {
                var errorMsg = new
                {
                    message = "Вход не выполнен",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                _logger.LogWarning("Login failed, model not valid");
                return Created("", errorMsg);
            }
        }

        [HttpPost]
        [Route("api/account/logoff")]
        public async Task<IActionResult> LogOff()
        {
            UserModel user = await _userService.GetCurrentUserAsync(HttpContext, _userManager);
            if (user == null)
            {
                _logger.LogWarning("User isn't log in");
                return Unauthorized(new { message = "Сначала выполните вход" });
            }

            // Удаление куки
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Logoff {0} succeeded", user.UserName);
            return Ok(new { message = "Выполнен выход", userName = user.UserName });
        }

        [HttpGet]
        [Route("api/account/isauthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            UserModel user = await _userService.GetCurrentUserAsync(HttpContext, _userManager);
            if (user == null)
            {
                _logger.LogInformation("User isn't authenticated");
                return Unauthorized(new { message = "Вы Гость. Пожалуйста, выполните вход" });
            }

            string? userRole = await _userService.GetUserRole(user.UserName, _userManager);
            _logger.LogInformation("{0} with role {1} is authenticated", user.UserName, userRole);
            return Ok(new { message = "Сессия активна", userName = user.UserName, userRole });
        }
    }
}
