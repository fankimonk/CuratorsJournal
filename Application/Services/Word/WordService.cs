using Application.Entities;
using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Interfaces.PageRepositories;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Application.Services.Word
{
    public class WordService(IJournalsService journalsService, IPagesRepository pagesRepository,
        IHolidaysRepository holidaysRepository, IGroupsRepository groupsRepository) : IWordService
    {
        private readonly IJournalsService _journalsService = journalsService;
        private readonly IPagesRepository _pagesRepository = pagesRepository;

        private readonly IHolidaysRepository _holidaysRepository = holidaysRepository;

        private readonly IGroupsRepository _groupsRepository = groupsRepository;

        public async Task<FileData?> GenerateWord(int journalId)
        {
            string filePath = "example.docx";

            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();

                Body body = new Body();

                var titlePageGenerator = new TitlePageGenerator(journalId, body, _journalsService);
                try { await titlePageGenerator.Generate(); }
                catch { return null; }

                var contactPhonesPageGenerator = new ContactPhonesPageGenerator(journalId, body, _pagesRepository);
                try { await contactPhonesPageGenerator.Generate(); }
                catch { return null; }

                var holidaysPageGenerator = new HolidaysPageGenerator(journalId, body, _pagesRepository, _holidaysRepository);
                try { await holidaysPageGenerator.Generate(); }
                catch { return null; }

                var socioPedagogicalCharacteristicsPageGenerator = new SocioPedagogicalCharacteristicsPageGenerator(journalId, body, _pagesRepository, _groupsRepository);
                try { await socioPedagogicalCharacteristicsPageGenerator.Generate(); }
                catch { return null; }

                var educationalProcessSchedulePageGenerator = new EducationalProcessSchedulePageGenerator(journalId, body, _pagesRepository);
                try { await educationalProcessSchedulePageGenerator.Generate(); }
                catch { return null; }

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }

            if (!File.Exists(filePath)) return null;

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            var contentType = "application/octet-stream";
            return new FileData(memory, contentType, filePath);
        }
    }
}
