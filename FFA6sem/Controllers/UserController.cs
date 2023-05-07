using DAL.Entities;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FFA6sem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IDbCrud _dbCrud;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserController> _logger;

        public UserController(IDbCrud dbCrud, IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<UserController> logger)
        {
            _dbCrud = dbCrud;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
        {
            var users = await _dbCrud.GetAllUsers(_userManager);
            _logger.LogInformation("GetAllUsers called");
            return users;
        }

        // GET api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(string id)
        {
            var user = await _dbCrud.GetUserById(id, _userManager);

            if (user == null)
            {
                _logger.LogWarning("User with id {0} not found", id);
                return NotFound();
            }
            _logger.LogInformation("User with id {0} found", id);
            return user;
        }

    }
}
