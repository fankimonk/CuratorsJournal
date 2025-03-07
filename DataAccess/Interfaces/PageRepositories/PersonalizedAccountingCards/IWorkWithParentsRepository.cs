using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards
{
    public interface IWorkWithParentsRepository
    {
        Task<List<WorkWithParentsRecord>?> GetByCardIdAsync(int cardId);
        Task<WorkWithParentsRecord?> CreateAsync(WorkWithParentsRecord record);
        Task<WorkWithParentsRecord?> UpdateAsync(int id, WorkWithParentsRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
