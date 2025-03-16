using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IChronicDiseasesRepository
    {
        Task<List<ChronicDisease>> GetAllAsync();
        Task<ChronicDisease?> CreateAsync(ChronicDisease disease);
        Task<ChronicDisease?> UpdateAsync(int id, ChronicDisease disease);
        Task<bool> DeleteAsync(int id);
    }
}
