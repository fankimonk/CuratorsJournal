using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards
{
    public interface IParentalInformationRepository
    {
        Task<List<ParentalInformationRecord>?> GetByCardIdAsync(int cardId);
        Task<ParentalInformationRecord?> CreateAsync(ParentalInformationRecord record);
        Task<ParentalInformationRecord?> UpdateAsync(int id, ParentalInformationRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
