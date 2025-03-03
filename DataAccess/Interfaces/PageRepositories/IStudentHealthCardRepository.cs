using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface IStudentHealthCardRepository : IPageRepositoryBase
    {
        Task<List<StudentsHealthCardRecord>?> GetByPageIdAsync(int pageId);
        Task<StudentsHealthCardRecord?> CreateAsync(StudentsHealthCardRecord record);
        Task<StudentsHealthCardRecord?> UpdateAsync(int id, StudentsHealthCardRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
