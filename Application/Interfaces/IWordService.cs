using Application.Entities;

namespace Application.Interfaces
{
    public interface IWordService
    {
        Task<FileData?> GenerateWord(int journalId);
    }
}