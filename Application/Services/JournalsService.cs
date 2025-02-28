using Application.Interfaces;
using DataAccess.Interfaces;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services
{
    public class JournalsService : IJournalsService
    {
        private readonly IJournalsRepository _journalsRepository;

        public JournalsService(IJournalsRepository journalsRepository)
        {
            _journalsRepository = journalsRepository;
        }

        public async Task<Journal?> CreateJournal(int groupId)
        {
            var journal = new Journal { GroupId = groupId };

            var pages = Enum
                .GetValues<PageTypes>()
                .Select(p => new Page
                {
                    PageTypeId = (int)p,
                    Journal = journal
                }).ToList();

            journal.Pages = pages;

            return await _journalsRepository.CreateAsync(journal);
        }

        public async Task<Journal?> GetJournalsTitlePageData(int journalId)
        {
            return await _journalsRepository.GetById(journalId);
        }
    }
}
