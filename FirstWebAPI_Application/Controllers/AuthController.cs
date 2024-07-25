using FirstWebAPI_Application.Models;
using FirstWebAPI_Application.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPI_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ILogger<AuthController> logger, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenRepository = tokenRepository;
        }

        //Post method for registering user first 
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.UserName
            };

            var identityResult = await _userManager.CreateAsync(identityUser, user.Password);

            if (identityResult.Succeeded)
            {

                if (user.Roles != null && user.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, user.Roles);

                }

                if (identityResult.Succeeded)
                {
                    _logger.LogInformation("User created successfully");
                    return Ok("User successfully created ");
                }
            }

            return BadRequest("There is error while registering the user");
        }

        //Post request // login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTOcs user)
        {
            var loggedInUser = await _userManager.FindByEmailAsync(user.UserName);

            if (loggedInUser != null)
            {
                var loggedInUserPasswordAuthentification = await _userManager
                    .CheckPasswordAsync(loggedInUser, user.Password);

                if (loggedInUserPasswordAuthentification)
                {
                    var roles = await _userManager.GetRolesAsync(loggedInUser);

                    if (roles != null && roles.Any())
                    {
                        var token = _tokenRepository.CreateJWTToken(loggedInUser, roles.ToList());
                        _logger.LogWarning("User Logged in successfully");
                        return Ok($"User Logged in successfully - {token}");
                    }
                }
            }

            _logger.LogWarning("Incorrect user name or password !!");

            return BadRequest("User or Password is invalid !!");
        }
    }
}
