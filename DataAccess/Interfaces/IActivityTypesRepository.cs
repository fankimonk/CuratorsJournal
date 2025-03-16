using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IActivityTypesRepository
    {
        Task<List<ActivityType>> GetAllAsync();
        Task<ActivityType?> CreateAsync(ActivityType position);
        Task<ActivityType?> UpdateAsync(int id, ActivityType position);
        Task<bool> DeleteAsync(int id);
    }
}
