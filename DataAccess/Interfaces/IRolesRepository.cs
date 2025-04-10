using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IRolesRepository
    {
        Task<List<Role>> GetAllAsync();
    }
}
