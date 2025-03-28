using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class SocioPedagogicalCharacteristicsRepository(CuratorsJournalDBContext dBContext)
        : PageRepositoryBase(dBContext), ISocioPedagogicalCharacteristicsRepository
    {
        public async Task<SocioPedagogicalCharacteristics?> GetByPageIdAsync(int id)
        {
            var pageExists = await PageExists(id);
            if (!pageExists) return null;
            return await _dbContext.SocioPedagogicalCharacteristics.AsNoTracking()
                .Include(s => s.Page).ThenInclude(p => p!.SocioPedagogicalCharacteristicsPageAttributes).ThenInclude(s => s!.AcademicYear)
                .FirstOrDefaultAsync(c => c.PageId == id);
        }

        public async Task<SocioPedagogicalCharacteristics?> UpdateAsync(int id, SocioPedagogicalCharacteristics characteristics)
        {
            if (characteristics == null) return null;

            var characteristicsToUpdate = await _dbContext.SocioPedagogicalCharacteristics.FirstOrDefaultAsync(p => p.Id == id);
            if (characteristicsToUpdate == null) return null;

            characteristicsToUpdate.TotalStudents = characteristics.TotalStudents;
            characteristicsToUpdate.FemalesCount = characteristics.FemalesCount;
            characteristicsToUpdate.MalesCount = characteristics.MalesCount;
            characteristicsToUpdate.BRSMMembersCount = characteristics.BRSMMembersCount;
            characteristicsToUpdate.OrphansUnderagesCount = characteristics.OrphansUnderagesCount;
            characteristicsToUpdate.StudentsWithoutParentalCareUnderagesCount = characteristics.StudentsWithoutParentalCareUnderagesCount;
            characteristicsToUpdate.OrphansOrStudentsWithoutParentalCareAdults = characteristics.OrphansOrStudentsWithoutParentalCareAdults;
            characteristicsToUpdate.StudentsWithSpecialPsychophysicalDevelopmentNeeds = characteristics.StudentsWithSpecialPsychophysicalDevelopmentNeeds;
            characteristicsToUpdate.StudentsWithDisabledParentsOfGroups1and2 = characteristics.StudentsWithDisabledParentsOfGroups1and2;
            characteristicsToUpdate.StudentsFromLargeFamilies = characteristics.StudentsFromLargeFamilies;
            characteristicsToUpdate.StudentsFromSingleParentFamilies = characteristics.StudentsFromSingleParentFamilies;
            characteristicsToUpdate.StudentsFromRegionsAffectedByChernobylDisaster = characteristics.StudentsFromRegionsAffectedByChernobylDisaster;
            characteristicsToUpdate.StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution = characteristics.StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution;
            characteristicsToUpdate.NonResidentStudents = characteristics.NonResidentStudents;
            characteristicsToUpdate.StudentsLivingWithParents = characteristics.StudentsLivingWithParents;
            characteristicsToUpdate.StudentsLivingInADormitory = characteristics.StudentsLivingInADormitory;
            characteristicsToUpdate.StudentsLivingWithRelatives = characteristics.StudentsLivingWithRelatives;
            characteristicsToUpdate.StudentsLivingInPrivateApartments = characteristics.StudentsLivingInPrivateApartments;
            characteristicsToUpdate.OtherInformation = characteristics.OtherInformation;

            await _dbContext.SaveChangesAsync();
            return await GetByPageIdAsync(characteristicsToUpdate.PageId);
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.SocioPedagogicalCharacteristics);
    }
}
