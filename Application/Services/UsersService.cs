using Application.Helpers;
using Application.Interfaces;
using DataAccess.Interfaces;
using Domain.Enums;
using Domain.Entities;

namespace Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public UsersService(IUsersRepository usersRepository,
            IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<RegistrationResult> Register(string userName, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            if (await _usersRepository.UsernameExistsAsync(userName)) return RegistrationResult.UsernameTaken;

            var user = new User { UserName = userName, PasswordHash = hashedPassword, RoleId = (int)Roles.Admin };

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

            return AuthorizationResult.Succeed(_jwtProvider.GenerateToken(user));
        }
    }
}
