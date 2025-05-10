using Application.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IFilesService
    {
        Task<bool> CreateFile(int journalId, IFormFile file);
        Task<List<string>?> GetDocuments(int journalId);
        FileData? GetFile(int journalId, string fileName);
        bool DeleteFile(int journalId, string fileName);
    }
}