using Application.Interfaces;
using Domain.Enums;
using DataAccess.Interfaces;

namespace Application.Services
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IUsersRepository _usersRepo;

        public PermissionsService(IUsersRepository usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public Task<HashSet<Permissions>> GetPermissionsAsync(int id)
        {
            return _usersRepo.GetUserPermissions(id);
        }
    }
}
