using Microsoft.AspNetCore.Mvc;
using API.Contracts.User;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Contracts.User;
using Application.Authorization;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public AuthController(IUsersService usersService) 
        {
            _usersService = usersService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var registrationResult = await _usersService.Register(request.Username, request.Password, (int)request.RoleId, request.WorkerId);
            if (registrationResult.Error != null || registrationResult.User == null) return BadRequest(registrationResult.Error);

            var user = registrationResult.User;
            return Ok(new UserResponse(user.Id, user.UserName, new RoleResponse(user.Role!.Id, user.Role.Name), user.WorkerId, null, null));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _usersService.Login(request.Username, request.Password);

            if (result.Error != null || result.Token == null) return BadRequest(result.Error);

            var response = new AuthResponse(result.Token.AccessToken, result.Token.RefreshToken!.Token);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("verify")]
        public async Task<IActionResult> VerifyAuth()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId);
            Console.WriteLine("UserId: " + userId?.Value);
            return Ok();
        }

        [HttpPost("logout")]
        //[Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Logout()
        {
            //HttpContext.Response.Cookies.Delete("tasty-cookies");
            //return Ok();

            var refreshToken = Request.Cookies["refreshtoken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _usersService.Logout(refreshToken);
            }
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshtoken"];
            if (string.IsNullOrEmpty(refreshToken)) return BadRequest("Refresh token is null");

            var token = await _usersService.RefreshToken(refreshToken);
            if (token == null) return BadRequest();

            return Ok(new AuthResponse(token.AccessToken, token.RefreshToken!.Token));
        }
    }
}
