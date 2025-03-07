using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards
{
    public interface IStudentDisciplinaryResponsibilitiesRepository
    {
        Task<List<StudentDisciplinaryResponsibility>?> GetByCardIdAsync(int cardId);
        Task<StudentDisciplinaryResponsibility?> CreateAsync(StudentDisciplinaryResponsibility record);
        Task<StudentDisciplinaryResponsibility?> UpdateAsync(int id, StudentDisciplinaryResponsibility record);
        Task<bool> DeleteAsync(int id);
    }
}
