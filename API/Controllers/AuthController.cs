using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Contracts.User;
using Microsoft.AspNetCore.Authorization;
using Application.Authorization;
using Application.Interfaces;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUsersService _usersService;

        public AuthController(IUsersRepository usersRepo, IUsersService usersService) 
        {
            _usersRepository = usersRepo;
            _usersService = usersService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var registrationResult = await _usersService.Register(request.UserName, request.Password);
            if (registrationResult.Error != null || registrationResult.User == null) return BadRequest(registrationResult.Error);

            var user = registrationResult.User;
            return Ok(new UserResponse(user.Id, user.UserName, user.Role!.Name, null, null));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _usersService.Login(request.Username, request.Password);

            if (result.Error != null || result.Token == null) return BadRequest(result.Error);

            //Old
            //HttpContext.Response.Cookies.Append("tasty-cookies", result.Token);

            //Response.Cookies.Append("refreshtoken", result.Token.RefreshToken!.Token);

            var response = new AuthResponse(result.Token.AccessToken, result.Token.RefreshToken!.Token);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("verify")]
        public async Task<IActionResult> VerifyAuth()
        {
            //Old
            //var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId);
            //var user = await _usersRepository.GetByIdAsync(int.Parse(userId!.Value));
            //return Ok(new UserResponse(user!.Id, user!.UserName, user.Role!.Name));

            return Ok("Authorized");
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

            //Response.Cookies.Append("refreshtoken", token.RefreshToken!.Token);

            return Ok(new AuthResponse(token.AccessToken, token.RefreshToken!.Token));
        }
    }
}
