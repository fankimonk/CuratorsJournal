using Application.Interfaces;
using DataAccess.Interfaces;
using Domain.Entities;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace Application.Services
{
    public class JournalsService : IJournalsService
    {
        private readonly IJournalsRepository _journalsRepository;
        private readonly IPagesRepository _pagesRepository;

        public JournalsService(IJournalsRepository journalsRepository, IPagesRepository pagesRepository)
        {
            _journalsRepository = journalsRepository;
            _pagesRepository = pagesRepository;
        }

        public async Task<Journal?> CreateJournal(int groupId)
        {
            var createdJournal = await _journalsRepository.CreateAsync(new Journal { GroupId = groupId });
            if (createdJournal == null) return createdJournal;

            var pageTypes = Enum.GetValues<PageTypes>();
            foreach (var pt in pageTypes)
            {
                await _pagesRepository.CreateAsync(new Page { PageTypeId = (int)pt, JournalId = createdJournal.Id });
            }

            return createdJournal;
        }

        public async Task<Tuple<int, Journal>?> GetJournalsTitlePageData(int journalId)
        {
            var journal = await _journalsRepository.GetById(journalId);
            if (journal == null) return null;

            var titlePages = await _pagesRepository.GetJournalPagesByType(journalId, PageTypes.Title);
            if (titlePages == null || titlePages.Count == 0) return null;

            var titlePage = titlePages.First();

            return new(titlePage.Id, journal);
        }
    }
}
