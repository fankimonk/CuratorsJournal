using Application.Helpers;
using Application.Interfaces;
using DataAccess.Interfaces;
using Domain.Enums;
using Domain.Entities;
using Application.Entities;

namespace Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public UsersService(IUsersRepository usersRepository, IRefreshTokensRepository refreshTokensRepository,
            IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _usersRepository = usersRepository;
            _refreshTokensRepository = refreshTokensRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<RegistrationResult> Register(string userName, string password, int? workerId)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            if (await _usersRepository.UsernameExistsAsync(userName)) return RegistrationResult.UsernameTaken;

            var user = new User { UserName = userName, PasswordHash = hashedPassword, RoleId = (int)Roles.Admin, WorkerId = workerId };

            var createdUser = await _usersRepository.CreateAsync(user);
            if (createdUser == null) return RegistrationResult.FailedToCreate;

            return RegistrationResult.Succeed(createdUser);
        }

        public async Task<AuthorizationResult> Login(string userName, string password)
        {
            var user = await _usersRepository.GetByUsernameAsync(userName);
            if (user == null)
            {
                return AuthorizationResult.UserNotFound;
            }

            var result = _passwordHasher.Verify(password, user.PasswordHash);
            if (result == false)
            {
                return AuthorizationResult.WrongPassword;
            }

            var token = _jwtProvider.GenerateToken(user);

            await _refreshTokensRepository.DisableTokensByUserIdAsync(user.Id);
            await _refreshTokensRepository.AddAsync(token.RefreshToken!);

            return AuthorizationResult.Succeed(token);
        }

        public async Task<AuthToken?> RefreshToken(string token)
        {
            if (!await _refreshTokensRepository.IsTokenValid(token)) return null;

            var user = await _usersRepository.GetByRefreshToken(token);
            if (user == null) return null;

            var authToken = _jwtProvider.GenerateToken(user);

            await _refreshTokensRepository.DisableToken(token);
            await _refreshTokensRepository.AddAsync(authToken.RefreshToken!);

            return authToken;
        }

        public async Task Logout(string token)
        {
            await _refreshTokensRepository.DisableToken(token);
        }
    }
}
