using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IJournalsRepository
    {
        Task<List<Journal>?> GetAllAsync(int userId);
        Task<bool> VerifyAccess(int userId, int journalId);
        Task<Journal?> CreateAsync(Journal journal);
        Task<Journal?> GetById(int journalId);
    }
}
