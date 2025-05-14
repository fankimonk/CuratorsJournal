using Application.Entities;

namespace Application.Interfaces
{
    public interface IJournalKeepingService
    {
        Task<JournalKeeping?> Get();
        Task<JournalKeeping?> Update(JournalKeeping journalKeeping);
    }
}