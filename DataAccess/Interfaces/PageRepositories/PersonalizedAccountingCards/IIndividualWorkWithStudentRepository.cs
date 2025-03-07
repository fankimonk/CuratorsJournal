using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards
{
    public interface IIndividualWorkWithStudentRepository
    {
        Task<List<IndividualWorkWithStudentRecord>?> GetByCardIdAsync(int cardId);
        Task<IndividualWorkWithStudentRecord?> CreateAsync(IndividualWorkWithStudentRecord record);
        Task<IndividualWorkWithStudentRecord?> UpdateAsync(int id, IndividualWorkWithStudentRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
