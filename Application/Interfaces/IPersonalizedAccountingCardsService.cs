namespace Application.Interfaces
{
    public interface IPersonalizedAccountingCardsService
    {
        Task<List<int>?> SynchronizeStundets(int journalId);
    }
}