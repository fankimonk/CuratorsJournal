using DataAccess.Interfaces;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;
using Domain.Entities.JournalContent.Pages;
using Domain.Entities.JournalContent.Pages.Attributes;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PagesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IPagesRepository
    {
        public async Task<Page?> CreateAsync(Page page)
        {
            if (page == null) return null;
            if (!await PageTypeExists(page.PageTypeId)) return null;
            if (!await JournalExists(page.JournalId)) return null;

            switch(page.PageTypeId)
            {
                case (int)PageTypes.PsychologicalAndPedagogicalCharacteristicsPage:
                    page.PsychologicalAndPedagogicalCharacteristics = new PsychologicalAndPedagogicalCharacteristics();
                    break;

                case (int)PageTypes.DynamicsOfKeyIndicatorsPage:
                    var keyIndicators = Enum.GetValues<KeyIndicators>();
                    foreach (var ki in keyIndicators)
                    {
                        page.DynamicsOfKeyIndicators
                            .Add(new DynamicsOfKeyIndicatorsRecord { KeyIndicatorId = (int)ki });
                    }
                    break;

                case (int)PageTypes.SocioPedagogicalCharacteristicsPage:
                    page.SocioPedagogicalCharacteristics = new SocioPedagogicalCharacteristics();
                    page.SocioPedagogicalCharacteristicsPageAttributes = new SocioPedagogicalCharacteristicsPageAttributes();
                    break;

                case (int)PageTypes.PersonalizedAccountingCardPage:
                    page.PersonalizedAccountingCard = new PersonalizedAccountingCard();
                    break;

                case (int)PageTypes.StudentsHealthCardPage:
                    page.HealthCardPageAttributes = new HealthCardPageAttributes();
                    break;

                case (int)PageTypes.CuratorsIdeologicalAndEducationalWorkAccountingPage:
                    page.CuratorsIdeologicalAndEducationalWorkPageAttributes = new CuratorsIdeologicalAndEducationalWorkPageAttributes();
                    break;

                case (int)PageTypes.GroupActivesPage:
                    page.GroupActives.Add(new GroupActive { PositionName = "Староста" });
                    page.GroupActives.Add(new GroupActive { PositionName = "Заместитель старосты" });
                    page.GroupActives.Add(new GroupActive { PositionName = "Профорг" });
                    break;

                default:
                    break;
            }

            var createdPage = await _dbContext.Pages.AddAsync(page);

            await _dbContext.SaveChangesAsync();
            return createdPage.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Pages.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Page?> GetById(int id)
        {
            return await _dbContext.Pages.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Page>> GetByJournalId(int journalId)
        {
            return await _dbContext.Pages.AsNoTracking().Include(p => p.PageType).Where(p => p.JournalId == journalId).ToListAsync();
        }

        private async Task<bool> PageTypeExists(int id) =>
            await _dbContext.PageTypes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> JournalExists(int id) =>
            await _dbContext.Journals.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
