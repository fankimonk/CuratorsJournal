using DataAccess.Interfaces;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;
using Domain.Entities.JournalContent.FinalPerformanceAccounting;
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
                case (int)PageTypes.PsychologicalAndPedagogicalCharacteristics:
                    page.PsychologicalAndPedagogicalCharacteristics = new PsychologicalAndPedagogicalCharacteristics();
                    break;

                case (int)PageTypes.DynamicsOfKeyIndicators:
                    var keyIndicators = Enum.GetValues<KeyIndicators>();
                    foreach (var ki in keyIndicators)
                    {
                        var record = new DynamicsOfKeyIndicatorsRecord { KeyIndicatorId = (int)ki };
                        record.KeyIndicatorsByCourse.Add(new KeyIndicatorByCourse { Course = 1, Value = null });
                        page.DynamicsOfKeyIndicators.Add(record);
                    }
                    break;

                case (int)PageTypes.SocioPedagogicalCharacteristics:
                    page.SocioPedagogicalCharacteristics = new SocioPedagogicalCharacteristics();
                    page.SocioPedagogicalCharacteristicsPageAttributes = new SocioPedagogicalCharacteristicsPageAttributes();
                    break;

                case (int)PageTypes.PersonalizedAccountingCard:
                    page.PersonalizedAccountingCard ??= new PersonalizedAccountingCard();
                    break;

                case (int)PageTypes.StudentsHealthCard:
                    page.HealthCardPageAttributes = new HealthCardPageAttributes();
                    break;

                case (int)PageTypes.CuratorsIdeologicalAndEducationalWorkAccounting:
                    page.CuratorsIdeologicalAndEducationalWorkPageAttributes = new CuratorsIdeologicalAndEducationalWorkPageAttributes();
                    break;

                case (int)PageTypes.GroupActives:
                    page.GroupActives.Add(new GroupActive { PositionName = "Староста" });
                    page.GroupActives.Add(new GroupActive { PositionName = "Заместитель старосты" });
                    page.GroupActives.Add(new GroupActive { PositionName = "Профорг" });
                    break;

                case (int)PageTypes.FinalPerformanceAccounting:
                    for (int i = 0; i < 6; i++)
                    {
                        page.PerformanceAccountingColumns.Add(new PerformanceAccountingColumn { CertificationTypeId = 1, SubjectId = null });
                        page.PerformanceAccountingColumns.Add(new PerformanceAccountingColumn { CertificationTypeId = 2, SubjectId = null });
                    }
                    break;

                default:
                    break;
            }

            var createdPage = await _dbContext.Pages.AddAsync(page);

            await _dbContext.SaveChangesAsync();
            return await _dbContext.Pages.Include(p => p.PageType).AsNoTracking().FirstOrDefaultAsync(p => p.Id == createdPage.Entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var page = await _dbContext.Pages.AsNoTracking().Include(p => p.PersonalizedAccountingCard).FirstOrDefaultAsync(p => p.Id == id);
            if (page == null) return false;
            if (page.PersonalizedAccountingCard != null && page.PersonalizedAccountingCard.StudentId != null)
            {
                var studentListRecord = await _dbContext.StudentList.FirstOrDefaultAsync(r => r.StudentId == page.PersonalizedAccountingCard.StudentId);
                if (studentListRecord != null)
                {
                    studentListRecord.PersonalizedAccountingCardId = null;
                    await _dbContext.SaveChangesAsync();
                }
            }

            var deletedRows = await _dbContext.Pages.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Page?> GetById(int id)
        {
            return await _dbContext.Pages.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Page>?> GetByJournalId(int journalId)
        {
            if (!await JournalExists(journalId)) return null;
            return await _dbContext.Pages.AsNoTracking().Include(p => p.PageType).Where(p => p.JournalId == journalId).ToListAsync();
        }

        public async Task<List<PageType>?> GetByJournalIdGroupedByTypes(int journalId)
        {
            if (!await JournalExists(journalId)) return null;
            return await _dbContext.PageTypes.Include(pt => pt.Pages).ThenInclude(p => p.PersonalizedAccountingCard).ThenInclude(c => c!.Student).AsNoTracking()
                .Select(pt => new PageType
                {
                    Id = pt.Id,
                    Name = pt.Name,
                    MaxPages = pt.MaxPages,
                    Pages = pt.Pages.Where(p => p.JournalId == journalId).ToList()
                }).ToListAsync();
        }

        public async Task<List<Page>?> GetJournalPagesByType(int journalId, PageTypes pageType)
        {
            if (!await JournalExists(journalId)) return null;
            if (!await PageTypeExists((int)pageType)) return null;

            var pages = _dbContext.Pages.AsNoTracking();
            switch(pageType)
            {
                case PageTypes.ContactPhones:
                    pages = pages.Include(p => p.ContactPhoneNumbers);
                    break;

                case PageTypes.SocioPedagogicalCharacteristics:
                    pages = pages.Include(p => p.SocioPedagogicalCharacteristics)
                        .Include(p => p.SocioPedagogicalCharacteristicsPageAttributes)
                        .ThenInclude(s => s!.AcademicYear);
                    break;

                case PageTypes.EducationalProcessSchedule:
                    pages = pages.Include(p => p.EducationalProcessSchedule);
                    break;

                case PageTypes.DynamicsOfKeyIndicators:
                    pages = pages.Include(p => p.DynamicsOfKeyIndicators)
                        .ThenInclude(d => d.KeyIndicatorsByCourse);
                    pages = pages.Include(p => p.DynamicsOfKeyIndicators)
                        .ThenInclude(d => d.KeyIndicator);
                    break;

                case PageTypes.GroupActives:
                    pages = pages.Include(p => p.GroupActives)
                        .ThenInclude(a => a.Student);
                    break;

                case PageTypes.StudentList:
                    pages = pages.Include(p => p.StudentList)
                        .ThenInclude(l => l.Student);
                    break;

                case PageTypes.PersonalizedAccountingCard:
                    pages = pages.Include(p => p.PersonalizedAccountingCard)
                        .ThenInclude(c => c!.Student).ThenInclude(s => s!.PEGroups);
                    pages = pages.Include(p => p.PersonalizedAccountingCard)
                        .ThenInclude(c => c!.Student).ThenInclude(s => s!.ChronicDiseases);
                    pages = pages.Include(p => p.PersonalizedAccountingCard)
                        .ThenInclude(c => c!.ParentalInformation);
                    pages = pages.Include(p => p.PersonalizedAccountingCard)
                        .ThenInclude(c => c!.IndividualInformation);
                    pages = pages.Include(p => p.PersonalizedAccountingCard)
                        .ThenInclude(c => c!.StudentEcouragements);
                    pages = pages.Include(p => p.PersonalizedAccountingCard)
                        .ThenInclude(c => c!.StudentDisciplinaryResponsibilities);
                    pages = pages.Include(p => p.PersonalizedAccountingCard)
                        .ThenInclude(c => c!.IndividualWorkWithStudent);
                    pages = pages.Include(p => p.PersonalizedAccountingCard)
                        .ThenInclude(c => c!.WorkWithParents);
                    break;

                case PageTypes.StudentsHealthCard:
                    pages = pages.Include(p => p.StudentsHealthCards)
                        .ThenInclude(c => c.Student)
                        .Include(p => p.HealthCardPageAttributes)
                        .ThenInclude(a => a!.AcademicYear);
                    break;

                case PageTypes.FinalPerformanceAccounting:
                    pages = pages.Include(p => p.FinalPerformanceAccounting)
                        .ThenInclude(r => r.Student)
                        .Include(p => p.FinalPerformanceAccounting)
                        .ThenInclude(r => r.PerformanceAccountingGrades);
                    break;

                case PageTypes.CuratorsIdeologicalAndEducationalWorkAccounting:
                    pages = pages.Include(p => p.CuratorsIdeologicalAndEducationalWorkAccounting)
                        .Include(p => p.CuratorsIdeologicalAndEducationalWorkPageAttributes);
                    break;

                case PageTypes.InformationHoursAccounting:
                    pages = pages.Include(p => p.InformationHoursAccounting);
                    break;

                case PageTypes.CuratorsParticipationInPedagogicalSeminars:
                    pages = pages.Include(p => p.CuratorsParticipationInPedagogicalSeminars);
                    break;

                case PageTypes.LiteratureWork:
                    pages = pages.Include(p => p.LiteratureWork)
                        .ThenInclude(l => l.Literature);
                    break;

                case PageTypes.PsychologicalAndPedagogicalCharacteristics:
                    pages = pages.Include(p => p.PsychologicalAndPedagogicalCharacteristics)
                        .ThenInclude(c => c!.Worker)
                        .ThenInclude(w => w!.Position);
                    break;

                case PageTypes.RecomendationsAndRemarks:
                    pages = pages.Include(p => p.RecomendationsAndRemarks)
                        .ThenInclude(r => r.Reviewer);
                    break;

                case PageTypes.Traditions:
                    pages = pages.Include(p => p.Traditions);
                    break;
            }

            return await pages.Where(p => p.JournalId == journalId && p.PageTypeId == (int)pageType)
                .ToListAsync();
        }

        private async Task<bool> PageTypeExists(int id) =>
            await _dbContext.PageTypes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> JournalExists(int id) =>
            await _dbContext.Journals.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        public async Task<bool?> ToggleIsApproved(int pageId)
        {
            var page = await _dbContext.Pages.FirstOrDefaultAsync(p => p.Id == pageId);
            if (page == null) return null;

            page.IsApproved = !page.IsApproved;
            await _dbContext.SaveChangesAsync();
            return page.IsApproved;
        }

        public async Task<bool> ApproveAllJournalPagesAsync(int journalId)
        {
            if (!await JournalExists(journalId)) return false;
            var pages = _dbContext.Pages.Include(p => p.PageType).Where(p => p.JournalId == journalId);
            await pages.ForEachAsync(p => p.IsApproved = true);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnapproveAllJournalPagesAsync(int journalId)
        {
            if (!await JournalExists(journalId)) return false;
            var pages = _dbContext.Pages.Include(p => p.PageType).Where(p => p.JournalId == journalId);
            await pages.ForEachAsync(p => p.IsApproved = false);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
