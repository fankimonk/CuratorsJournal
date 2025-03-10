using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards
{
    public interface IPersonalizedAccountingCardsRepository : IPageRepositoryBase
    {
        Task<PersonalizedAccountingCard?> GetByPageIdAsync(int pageId);
        Task<PersonalizedAccountingCard?> CreateAsync(PersonalizedAccountingCard characteristics);
        Task<PersonalizedAccountingCard?> UpdateAsync(int id, PersonalizedAccountingCard characteristics);
        Task<bool> DeleteAsync(int id);
        Task<List<PersonalizedAccountingCard>> GetByJournalId(int id);
    }
}
