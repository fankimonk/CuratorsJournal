using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards
{
    public interface IIndividualInformationRepository
    {
        Task<List<IndividualInformationRecord>?> GetByCardIdAsync(int cardId);
        Task<IndividualInformationRecord?> CreateAsync(IndividualInformationRecord record);
        Task<IndividualInformationRecord?> UpdateAsync(int id, IndividualInformationRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
