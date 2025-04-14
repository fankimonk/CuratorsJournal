using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Domain.Entities.JournalContent.Pages;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Domain.Enums.Journal;

namespace Application.Services
{
    public class PersonalizedAccountingCardsService(IPersonalizedAccountingCardsRepository cardsRepository,
        IPagesRepository pagesRepository) : IPersonalizedAccountingCardsService
    {
        private readonly IPersonalizedAccountingCardsRepository _cardsRepository = cardsRepository;
        private readonly IPagesRepository _pagesRepository = pagesRepository;

        public async Task<List<int>?> SynchronizeStundets(int journalId)
        {
            var studentsWithoutCards = await _cardsRepository.GetStudentIdsThatDontHaveCard(journalId);
            if (studentsWithoutCards == null) return null;

            foreach (var id in studentsWithoutCards)
            {
                await _pagesRepository.CreateAsync(new Page
                {
                    PageTypeId = (int)PageTypes.PersonalizedAccountingCard,
                    JournalId = journalId,
                    IsApproved = false,
                    PersonalizedAccountingCard = new PersonalizedAccountingCard { StudentId = id }
                });
            }

            return studentsWithoutCards;
        }
    }
}
