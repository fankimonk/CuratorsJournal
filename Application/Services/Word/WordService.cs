using Application.Entities;
using Application.Interfaces;
using Application.Services.Word.PesonalizedAccountingCard;
using Application.Utils;
using DataAccess.Interfaces;
using DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class WordService(IJournalsService journalsService, IPagesRepository pagesRepository,
        IHolidaysRepository holidaysRepository, IGroupsRepository groupsRepository,
        IPerformanceAccountingColumnsRepository performanceAccountingColumnsRepository) : IWordService
    {
        private readonly IJournalsService _journalsService = journalsService;
        private readonly IPagesRepository _pagesRepository = pagesRepository;

        private readonly IHolidaysRepository _holidaysRepository = holidaysRepository;

        private readonly IGroupsRepository _groupsRepository = groupsRepository;

        private readonly IPerformanceAccountingColumnsRepository _performanceAccountingColumnsRepository = performanceAccountingColumnsRepository;

        public async Task<FileData?> GenerateWord(int journalId)
        {
            string filePath = "journal.docx";

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

                var dynamicsOfKeyIndicatorsPageGenerator = new DynamicsOfKeyIndicatorsPageGenerator(journalId, body, _pagesRepository);
                try { await dynamicsOfKeyIndicatorsPageGenerator.Generate(); }
                catch { return null; }

                var groupActivesPageGenerator = new GroupActivesPageGenerator(journalId, body, _pagesRepository);
                try { await groupActivesPageGenerator.Generate(); }
                catch { return null; }

                var studentListPageGenerator = new StudentListPageGenerator(journalId, body, _pagesRepository);
                try { await studentListPageGenerator.Generate(); }
                catch { return null; }

                var personalizedAccountingCardPageGenerator = new PersonalizedAccountingCardPageGenerator(journalId, body, _pagesRepository);
                try { await personalizedAccountingCardPageGenerator.Generate(); }
                catch { return null; }

                WordUtils.AppendSectionBreak(WordUtils.PageOrientationTypes.Portrait, body);

                var studentHealthCardPageGenerator = new StudentHealthCardPageGenerator(journalId, body, _pagesRepository);
                try { await studentHealthCardPageGenerator.Generate(); }
                catch { return null; }

                var finalPerformaceAccountingPageGenerator = new FinalPerformanceAccountingPageGenerator(journalId, body, _pagesRepository, _performanceAccountingColumnsRepository);
                try { await finalPerformaceAccountingPageGenerator.Generate(); }
                catch { return null; }

                var ideologicalAndEducationalWorkAccountingPageGenerator = new IdeologicalAndEducationalWorkAccountingPageGenerator(journalId, body, _pagesRepository);
                try { await ideologicalAndEducationalWorkAccountingPageGenerator.Generate(); }
                catch { return null; }

                var informationHoursAccountingPageGenerator = new InformationHoursAccountingPageGenerator(journalId, body, _pagesRepository);
                try { await informationHoursAccountingPageGenerator.Generate(); }
                catch { return null; }

                var curatorsParticipationPageGenerator = new CuratorsParticipationPageGenerator(journalId, body, _pagesRepository);
                try { await curatorsParticipationPageGenerator.Generate(); }
                catch { return null; }

                var literatureWorkPageGenerator = new LiteratureWorkPageGenerator(journalId, body, _pagesRepository);
                try { await literatureWorkPageGenerator.Generate(); }
                catch { return null; }

                var psychologicalAndPedagogicalCharacteristicsPageGenerator = new PsychologicalAndPedagogicalCharacteristicsPageGenerator(journalId, body, _pagesRepository);
                try { await psychologicalAndPedagogicalCharacteristicsPageGenerator.Generate(); }
                catch { return null; }

                WordUtils.AppendSectionBreak(WordUtils.PageOrientationTypes.Portrait, body);

                var recommendationsAndRemarksPageGenerator = new RecommendationsAndRemarksPageGenerator(journalId, body, _pagesRepository);
                try { await recommendationsAndRemarksPageGenerator.Generate(); }
                catch { return null; }

                var traditionPageGenerator = new TraditionsPageGenerator(journalId, body, _pagesRepository);
                try { await traditionPageGenerator.Generate(); }
                catch { return null; }

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }

            return GetFileData(filePath);
        }

        public async Task<FileData?> GeneratePage(int journalId, int pageId)
        {
            var page = await _pagesRepository.GetPageDataByIdAsync(pageId);
            if (page == null) return null;

            string filePath = "page.docx";

            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();

                Body body = new Body();

                switch (page.PageTypeId)
                {
                    case (int)PageTypes.Title:
                        var titlePageGenerator = new TitlePageGenerator(journalId, body, _journalsService);
                        try { await titlePageGenerator.Generate(); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.ContactPhones:
                        var contactPhonesPageGenerator = new ContactPhonesPageGenerator(journalId, body, _pagesRepository);
                        try { await contactPhonesPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.Holidays:
                        var holidaysPageGenerator = new HolidaysPageGenerator(journalId, body, _pagesRepository, _holidaysRepository);
                        try { await holidaysPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.SocioPedagogicalCharacteristics:
                        var socioPedagogicalCharacteristicsPageGenerator = new SocioPedagogicalCharacteristicsPageGenerator(journalId, body, _pagesRepository, _groupsRepository);
                        try { await socioPedagogicalCharacteristicsPageGenerator.Generate(); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.EducationalProcessSchedule:
                        var educationalProcessSchedulePageGenerator = new EducationalProcessSchedulePageGenerator(journalId, body, _pagesRepository);
                        try { await educationalProcessSchedulePageGenerator.Generate(); }
                        catch { return null; }
                        var dynamicsOfKeyIndicatorsPageGenerator = new DynamicsOfKeyIndicatorsPageGenerator(journalId, body, _pagesRepository);
                        try { await dynamicsOfKeyIndicatorsPageGenerator.Generate(); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.DynamicsOfKeyIndicators:
                        educationalProcessSchedulePageGenerator = new EducationalProcessSchedulePageGenerator(journalId, body, _pagesRepository);
                        try { await educationalProcessSchedulePageGenerator.Generate(); }
                        catch { return null; }
                        dynamicsOfKeyIndicatorsPageGenerator = new DynamicsOfKeyIndicatorsPageGenerator(journalId, body, _pagesRepository);
                        try { await dynamicsOfKeyIndicatorsPageGenerator.Generate(); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.GroupActives:
                        var groupActivesPageGenerator = new GroupActivesPageGenerator(journalId, body, _pagesRepository);
                        try { await groupActivesPageGenerator.Generate(); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.StudentList:
                        var studentListPageGenerator = new StudentListPageGenerator(journalId, body, _pagesRepository);
                        try { await studentListPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.PersonalizedAccountingCard:
                        var personalizedAccountingCardPageGenerator = new PersonalizedAccountingCardPageGenerator(journalId, body, _pagesRepository);
                        try { await personalizedAccountingCardPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.StudentsHealthCard:
                        var studentHealthCardPageGenerator = new StudentHealthCardPageGenerator(journalId, body, _pagesRepository);
                        try { await studentHealthCardPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.FinalPerformanceAccounting:
                        var finalPerformaceAccountingPageGenerator = new FinalPerformanceAccountingPageGenerator(journalId, body, _pagesRepository, _performanceAccountingColumnsRepository);
                        try { await finalPerformaceAccountingPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.CuratorsIdeologicalAndEducationalWorkAccounting:
                        var ideologicalAndEducationalWorkAccountingPageGenerator = new IdeologicalAndEducationalWorkAccountingPageGenerator(journalId, body, _pagesRepository);
                        try { await ideologicalAndEducationalWorkAccountingPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.InformationHoursAccounting:
                        var informationHoursAccountingPageGenerator = new InformationHoursAccountingPageGenerator(journalId, body, _pagesRepository);
                        try { await informationHoursAccountingPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.CuratorsParticipationInPedagogicalSeminars:
                        var curatorsParticipationPageGenerator = new CuratorsParticipationPageGenerator(journalId, body, _pagesRepository);
                        try { await curatorsParticipationPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.LiteratureWork:
                        var literatureWorkPageGenerator = new LiteratureWorkPageGenerator(journalId, body, _pagesRepository);
                        try { await literatureWorkPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.PsychologicalAndPedagogicalCharacteristics:
                        var psychologicalAndPedagogicalCharacteristicsPageGenerator = new PsychologicalAndPedagogicalCharacteristicsPageGenerator(journalId, body, _pagesRepository);
                        try { await psychologicalAndPedagogicalCharacteristicsPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.RecomendationsAndRemarks:
                        var recommendationsAndRemarksPageGenerator = new RecommendationsAndRemarksPageGenerator(journalId, body, _pagesRepository);
                        try { await recommendationsAndRemarksPageGenerator.Generate(page); }
                        catch { return null; }
                        break;

                    case (int)PageTypes.Traditions:
                        var traditionPageGenerator = new TraditionsPageGenerator(journalId, body, _pagesRepository);
                        try { await traditionPageGenerator.Generate(page); }
                        catch { return null; }
                        break;
                }

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }

            return GetFileData(filePath);
        }

        private FileData? GetFileData(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            var contentType = "application/octet-stream";
            var fileData = new FileData(memory, contentType, filePath);

            File.Delete(filePath);

            return fileData;
        }
    }
}
