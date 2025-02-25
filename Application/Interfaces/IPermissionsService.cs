using Domain.Enums;

namespace Application.Interfaces
{
    public interface IPermissionsService
    {
        Task<HashSet<Permissions>> GetPermissionsAsync(int id);
    }
}