using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoggingSystem.API.Controllers
{
    [Route("v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequstModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequstModel.Username,
                Email = registerRequstModel.Username
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequstModel.Password);

            if(identityResult.Succeeded)
            {
                if(registerRequstModel.Roles != null && registerRequstModel.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequstModel.Roles);

                    if(identityResult.Succeeded)
                    {
                        return Ok("User Created");
                    }
                }
            }

            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequstModel)
        {
            var user = await _userManager.FindByEmailAsync(loginRequstModel.Username);

            if (user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequstModel.Password);

                if (checkPasswordResult)
                {
                    // Create Token
                    return Ok();
                }
            }

            return BadRequest("Username or Password incorrect");
        }

    }
}
