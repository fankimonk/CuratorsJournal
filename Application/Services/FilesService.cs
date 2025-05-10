using Application.Entities;
using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class FilesService(IJournalsRepository journalsRepository) : IFilesService
    {
        private readonly IJournalsRepository _journalsRepository = journalsRepository;

        private readonly string _uploadDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "JournalsData");

        public async Task<bool> CreateFile(int journalId, IFormFile file)
        {
            if (file.Length == 0) return false;
            
            string journalDirectory = Path.Combine(_uploadDirectoryPath, journalId.ToString());
            if (!Directory.Exists(journalDirectory)) Directory.CreateDirectory(journalDirectory);

            var filePath = Path.Combine(journalDirectory, file.FileName);

            try
            {
                using (var stream = File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeleteFile(int journalId, string fileName)
        {
            string filePath = Path.Combine(_uploadDirectoryPath, journalId.ToString(), fileName);
            if (!File.Exists(filePath)) return false;
            File.Delete(filePath);
            return true;
        }

        public async Task<List<string>?> GetDocuments(int journalId)
        {
            if (!await _journalsRepository.Exists(journalId)) return null;
            string directoryPath = Path.Combine(_uploadDirectoryPath, journalId.ToString());
            if (!Directory.Exists(directoryPath)) return [];
            return Directory.GetFiles(directoryPath).Select(f => Path.GetFileName(f)).ToList();
        }

        public FileData? GetFile(int journalId, string fileName)
        {
            string filePath = Path.Combine(_uploadDirectoryPath, journalId.ToString(), fileName);
            if (!File.Exists(filePath)) return null;

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            var contentType = "application/octet-stream";
            var fileData = new FileData(memory, contentType, fileName);

            return fileData;
        }
    }
}
