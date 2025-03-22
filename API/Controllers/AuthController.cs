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
            return Ok(new UserResponse(user.Id, user.UserName, user.Role!.Name));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _usersService.Login(request.Username, request.Password);

            if (result.Error != null || string.IsNullOrEmpty(result.Token)) return BadRequest(result.Error);

            HttpContext.Response.Cookies.Append("tasty-cookies", result.Token);

            return Ok();
        }

        [HttpGet("check")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CheckAuth()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId);
            var user = await _usersRepository.GetByIdAsync(int.Parse(userId!.Value));
            return Ok(new UserResponse(user!.Id, user!.UserName, user.Role!.Name));
        }

        [HttpPost("logout")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("tasty-cookies");
            return Ok();
        }
    }
}
