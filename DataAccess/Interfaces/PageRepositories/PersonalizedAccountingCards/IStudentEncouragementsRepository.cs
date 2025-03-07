using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards
{
    public interface IStudentEncouragementsRepository
    {
        Task<List<StudentEcouragement>?> GetByCardIdAsync(int cardId);
        Task<StudentEcouragement?> CreateAsync(StudentEcouragement record);
        Task<StudentEcouragement?> UpdateAsync(int id, StudentEcouragement record);
        Task<bool> DeleteAsync(int id);
    }
}
