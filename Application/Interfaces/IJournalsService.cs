using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJournalsService
    {
        Task<Journal?> CreateJournal(int groupId);
        Task<Tuple<int, Journal>?> GetJournalsTitlePageData(int groupId);
    }
}
