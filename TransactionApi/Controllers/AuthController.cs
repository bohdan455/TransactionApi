using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using TransactionApi.Indentity.Interfaces;
using TransactionApi.Model;

namespace TransactionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<User> _userManager;

        public AuthController(IJwtTokenService jwtTokenService,
            SignInManager<User> signInManager,
            ILogger<AuthController> logger,
            UserManager<User> userManager)
        {
            _jwtTokenService = jwtTokenService;
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserModel userModel)
        {

            var result = await _signInManager.PasswordSignInAsync(userModel.Email,
                               userModel.Password, false, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                var token = await _jwtTokenService.GenerateJSONWebToken(userModel);
                return Ok( new { token } );
            }
            else if (result.IsLockedOut)
            {
                return BadRequest("User account locked out.");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserModel userModel)
        {
            var user = new User { UserName = userModel.Email, Email = userModel.Email };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                return Ok();
            }

            var errors = new List<string>();
            foreach (var error in result.Errors)
            {
               errors.Add(error.Description);
            }

            return BadRequest(new {errors});
        }
    }
}
