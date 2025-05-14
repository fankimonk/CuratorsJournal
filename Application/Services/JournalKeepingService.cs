using Application.Entities;
using Application.Interfaces;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Application.Services
{
    public class JournalKeepingService : IJournalKeepingService
    {
        private readonly string _journalKeepingFilePath = "JournalsData/JournalKeeping.json";

        public async Task<JournalKeeping?> Get()
        {
            if (!File.Exists(_journalKeepingFilePath)) return null;
            using FileStream openStream = File.OpenRead(_journalKeepingFilePath);
            var journalKeeping = await JsonSerializer.DeserializeAsync<JournalKeeping>(openStream);
            return journalKeeping;
        }

        public async Task<JournalKeeping?> Update(JournalKeeping journalKeeping)
        {
            using FileStream createStream = File.Create(_journalKeepingFilePath);
            try
            { 
                await JsonSerializer.SerializeAsync(createStream, journalKeeping, 
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                }); 
            }
            catch { return null; }
            return journalKeeping;
        }
    }
}
