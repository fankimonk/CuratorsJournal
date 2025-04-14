using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories.PersonalizedAccountingCards
{
    public class PersonalizedAccountingCardsRepository(CuratorsJournalDBContext dBContext)
        : PageRepositoryBase(dBContext), IPersonalizedAccountingCardsRepository
    {
        public async Task<PersonalizedAccountingCard?> CreateAsync(PersonalizedAccountingCard card)
        {
            if (card == null) return null;
            if (!await PageExists(card.PageId)) return null;
            if (card.StudentId != null && !await StudentExists((int)card.StudentId)) return null;

            var createdCard = await _dbContext.PersonalizedAccountingCards.AddAsync(card);

            await _dbContext.SaveChangesAsync();

            return createdCard.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.PersonalizedAccountingCards.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PersonalizedAccountingCard?> GetByPageIdAsync(int id)
        {
            var pageExists = await PageExists(id);
            if (!pageExists) return null;
            return await _dbContext.PersonalizedAccountingCards
                .AsNoTracking().FirstOrDefaultAsync(c => c.PageId == id);
        }

        public async Task<PersonalizedAccountingCard?> UpdateAsync(int id, PersonalizedAccountingCard card)
        {
            if (card == null) return null;
            if (card.StudentId != null && !await StudentExists((int)card.StudentId)) return null;

            var cardToUpdate = await _dbContext.PersonalizedAccountingCards.FirstOrDefaultAsync(p => p.Id == id);
            if (cardToUpdate == null) return null;

            cardToUpdate.BirthDate = card.BirthDate;
            cardToUpdate.PassportData = card.PassportData;
            cardToUpdate.Citizenship = card.Citizenship;
            cardToUpdate.GraduatedEducationalInstitution = card.GraduatedEducationalInstitution;
            cardToUpdate.ResidentialAddress = card.ResidentialAddress;
            cardToUpdate.StudentId = card.StudentId;

            await _dbContext.SaveChangesAsync();
            return cardToUpdate;
        }

        public async Task<List<PersonalizedAccountingCard>> GetByJournalId(int id)
        {
            return await _dbContext.PersonalizedAccountingCards
                .Include(c => c.Page).AsNoTracking()
                .Where(c => c.Page!.JournalId == id).ToListAsync();
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.PersonalizedAccountingCard);

        private async Task<bool> StudentExists(int id) =>
            await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> JournalExists(int id) =>
            await _dbContext.Journals.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id) != null;

        public async Task<int?> GetCardIdByStudentIdAsync(int studentId)
        {
            var card = await _dbContext.PersonalizedAccountingCards.AsNoTracking().FirstOrDefaultAsync(c => c.StudentId == studentId);
            if (card == null) return null;
            return card.Id;
        }

        public async Task<List<int>> GetStudentIdsThatHaveCard(int journalId)
        {
            return await _dbContext.PersonalizedAccountingCards
                .Include(c => c.Page).AsNoTracking()
                .Where(c => c.Page!.JournalId == journalId && c.StudentId != null)
                .Select(c => (int)c.StudentId).ToListAsync();
        }

        public async Task<List<int>?> GetStudentIdsThatDontHaveCard(int journalId)
        {
            if (!await JournalExists(journalId)) return null;

            var studentIds = await _dbContext.Students.Include(s => s.Group).ThenInclude(g => g!.Journal).AsNoTracking()
                .Where(s => s.Group!.Journal!.Id == journalId).Select(s => s.Id).ToListAsync();

            var stundetsWithCards = await GetStudentIdsThatHaveCard(journalId);
            return studentIds.Except(stundetsWithCards).ToList();
        }


    }
}
