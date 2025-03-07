using DataAccess.Interfaces;
using Domain.Entities.JournalContent.Pages.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SocioPedagogicalCharacteristicsAttributesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext),
        ISocioPedagogicalCharacteristicsAttributesRepository
    {
        public async Task<SocioPedagogicalCharacteristicsPageAttributes?> UpdateAcademicYear(int id, int? academicYearId)
        {
            if (academicYearId != null && !await AcademicYearExists((int)academicYearId)) return null;

            var attributes = await _dbContext.SocioPedagogicalCharacteristicsPageAttributes
                .FirstOrDefaultAsync(a => a.Id == id);
            if (attributes == null) return null;

            attributes.AcademicYearId = academicYearId;
            await _dbContext.SaveChangesAsync();

            return await _dbContext.SocioPedagogicalCharacteristicsPageAttributes
                .Include(s => s.AcademicYear).AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        private async Task<bool> AcademicYearExists(int id) =>
            await _dbContext.AcademicYears.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id) != null;
    }
}
