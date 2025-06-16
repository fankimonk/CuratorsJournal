using Application.Entities;

namespace Application.Interfaces
{
    public interface IWordService
    {
        Task<FileData?> GenerateWord(int journalId, bool hasPageNumeration);
        Task<FileData?> GeneratePage(int journalId, int pageId);
        Task<FileData?> GenerateJournalKeeping();
    }
}