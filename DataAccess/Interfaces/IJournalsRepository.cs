using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IJournalsRepository
    {
        Task<bool> ExistsAsync(int id);
        Task<bool?> CheckIsApprovedAsync(int id);
        Task<List<Journal>?> GetAllAsync(int userId);
        Task<bool> VerifyAccess(int userId, int journalId);
        Task<Journal?> CreateAsync(Journal journal);
        Task<Journal?> GetByIdAsync(int journalId);
    }
}
