using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface IStudentListRepository : IPageRepositoryBase
    {
        Task<List<StudentListRecord>?> GetByPageIdAsync(int pageId);
        Task<StudentListRecord?> CreateAsync(StudentListRecord record);
        Task<StudentListRecord?> UpdateAsync(int id, StudentListRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
