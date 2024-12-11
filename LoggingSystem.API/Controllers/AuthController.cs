using LoggingSystem.API.Action_Filters;
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
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [ValidateModel]
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

            return BadRequest(ModelState);
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
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles != null && roles.Any())
                    {
                        //Creating a Token
                        var token = _tokenRepository.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = token
                        };
                        // Return ok with the generated token
                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or Password incorrect");
        }


    }
}
