using Application.Interfaces;
using DataAccess.Interfaces;
using Domain.Entities;

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
            return await _journalsRepository.CreateAsync(journal);
        }

        public async Task<Journal?> GetJournalsTitlePageData(int journalId)
        {
            return await _journalsRepository.GetJournalWithTitlePageData(journalId);
        }
    }
}
