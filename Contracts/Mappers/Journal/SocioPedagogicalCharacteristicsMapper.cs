using Contracts.Journal.SocioPedagogicalCharacteristics;

namespace Contracts.Mappers.Journal
{
    public static class SocioPedagogicalCharacteristicsMapper
    {
        public static UpdateSocioPedagogicalCharacteristicsRequest ToRequest(this SocioPedagogicalCharacteristicsResponse characteristics)
        {
            return new UpdateSocioPedagogicalCharacteristicsRequest(
                characteristics.TotalStudents, characteristics.FemalesCount,
                characteristics.MalesCount, characteristics.BRSMMembersCount, characteristics.OrphansUnderagesCount,
                characteristics.StudentsWithoutParentalCareUnderagesCount, characteristics.OrphansOrStudentsWithoutParentalCareAdults,
                characteristics.StudentsWithSpecialPsychophysicalDevelopmentNeeds, characteristics.StudentsWithDisabledParentsOfGroups1and2,
                characteristics.StudentsFromLargeFamilies, characteristics.StudentsFromSingleParentFamilies,
                characteristics.StudentsFromRegionsAffectedByChernobylDisaster, characteristics.StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution,
                characteristics.NonResidentStudents, characteristics.StudentsLivingWithParents, characteristics.StudentsLivingInADormitory,
                characteristics.StudentsLivingWithRelatives, characteristics.StudentsLivingInPrivateApartments, characteristics.OtherInformation
            );
        }
    }
}
