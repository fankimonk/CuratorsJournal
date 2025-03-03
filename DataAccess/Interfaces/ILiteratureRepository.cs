using Domain.Entities.JournalContent.Literature;

namespace DataAccess.Interfaces
{
    public interface ILiteratureRepository
    {
        Task<List<LiteratureListRecord>> GetAllAsync();
        Task<LiteratureListRecord?> CreateAsync(LiteratureListRecord literature);
        Task<LiteratureListRecord?> UpdateAsync(int id, LiteratureListRecord literature);
        Task<bool> DeleteAsync(int id); 
    }
}
