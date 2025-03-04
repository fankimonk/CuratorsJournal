using Contracts.Journal.SocioPedagogicalCharacteristics;
using Domain.Entities.JournalContent;

namespace API.Mappers
{
    public static class SocioPedagogicalCharacteristicsMapper
    {
        public static SocioPedagogicalCharacteristicsResponse ToResponse(this SocioPedagogicalCharacteristics characteristics)
        {
            return new SocioPedagogicalCharacteristicsResponse(
                characteristics.Id, characteristics.TotalStudents, characteristics.FemalesCount,
                characteristics.MalesCount, characteristics.BRSMMembersCount, characteristics.OrphansUnderagesCount,
                characteristics.StudentsWithoutParentalCareUnderagesCount, characteristics.OrphansOrStudentsWithoutParentalCareAdults,
                characteristics.StudentsWithSpecialPsychophysicalDevelopmentNeeds, characteristics.StudentsWithDisabledParentsOfGroups1and2,
                characteristics.StudentsFromLargeFamilies, characteristics.StudentsFromSingleParentFamilies,
                characteristics.StudentsFromRegionsAffectedByChernobylDisaster, characteristics.StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution,
                characteristics.NonResidentStudents, characteristics.StudentsLivingWithParents, characteristics.StudentsLivingInADormitory,
                characteristics.StudentsLivingWithRelatives, characteristics.StudentsLivingInPrivateApartments, characteristics.OtherInformation,
                characteristics.PageId
            );
        }

        public static SocioPedagogicalCharacteristics ToEntity(this UpdateSocioPedagogicalCharacteristicsRequest request)
        {
            return new SocioPedagogicalCharacteristics
            {
                TotalStudents = request.TotalStudents,
                FemalesCount = request.FemalesCount,
                MalesCount = request.MalesCount,
                BRSMMembersCount = request.BRSMMembersCount,
                OrphansUnderagesCount = request.OrphansUnderagesCount,
                StudentsWithoutParentalCareUnderagesCount = request.StudentsWithoutParentalCareUnderagesCount,
                OrphansOrStudentsWithoutParentalCareAdults = request.OrphansOrStudentsWithoutParentalCareAdults,
                StudentsWithSpecialPsychophysicalDevelopmentNeeds = request.StudentsWithSpecialPsychophysicalDevelopmentNeeds,
                StudentsWithDisabledParentsOfGroups1and2 = request.StudentsWithDisabledParentsOfGroups1and2,
                StudentsFromLargeFamilies = request.StudentsFromLargeFamilies,
                StudentsFromSingleParentFamilies = request.StudentsFromSingleParentFamilies,
                StudentsFromRegionsAffectedByChernobylDisaster = request.StudentsFromRegionsAffectedByChernobylDisaster,
                StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution = request.StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution,
                NonResidentStudents = request.NonResidentStudents,
                StudentsLivingWithParents = request.StudentsLivingWithParents,
                StudentsLivingInADormitory = request.StudentsLivingInADormitory,
                StudentsLivingWithRelatives = request.StudentsLivingWithRelatives,
                StudentsLivingInPrivateApartments = request.StudentsLivingInPrivateApartments,
                OtherInformation = request.OtherInformation
            };
        }

        public static SocioPedagogicalCharacteristics ToEntity(this CreateSocioPedagogicalCharacteristicsRequest request)
        {
            return new SocioPedagogicalCharacteristics
            {
                TotalStudents = request.TotalStudents,
                FemalesCount = request.FemalesCount,
                MalesCount = request.MalesCount,
                BRSMMembersCount = request.BRSMMembersCount,
                OrphansUnderagesCount = request.OrphansUnderagesCount,
                StudentsWithoutParentalCareUnderagesCount = request.StudentsWithoutParentalCareUnderagesCount,
                OrphansOrStudentsWithoutParentalCareAdults = request.OrphansOrStudentsWithoutParentalCareAdults,
                StudentsWithSpecialPsychophysicalDevelopmentNeeds = request.StudentsWithSpecialPsychophysicalDevelopmentNeeds,
                StudentsWithDisabledParentsOfGroups1and2 = request.StudentsWithDisabledParentsOfGroups1and2,
                StudentsFromLargeFamilies = request.StudentsFromLargeFamilies,
                StudentsFromSingleParentFamilies = request.StudentsFromSingleParentFamilies,
                StudentsFromRegionsAffectedByChernobylDisaster = request.StudentsFromRegionsAffectedByChernobylDisaster,
                StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution = request.StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution,
                NonResidentStudents = request.NonResidentStudents,
                StudentsLivingWithParents = request.StudentsLivingWithParents,
                StudentsLivingInADormitory = request.StudentsLivingInADormitory,
                StudentsLivingWithRelatives = request.StudentsLivingWithRelatives,
                StudentsLivingInPrivateApartments = request.StudentsLivingInPrivateApartments,
                OtherInformation = request.OtherInformation,
                PageId = request.PageId
            };
        }
    }
}
