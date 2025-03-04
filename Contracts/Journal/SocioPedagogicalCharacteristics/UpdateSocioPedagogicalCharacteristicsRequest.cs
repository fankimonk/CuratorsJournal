namespace Contracts.Journal.SocioPedagogicalCharacteristics
{
    public record UpdateSocioPedagogicalCharacteristicsRequest
    (
        int? TotalStudents,
        int? FemalesCount,
        int? MalesCount,
        int? BRSMMembersCount,
        int? OrphansUnderagesCount,
        int? StudentsWithoutParentalCareUnderagesCount,
        int? OrphansOrStudentsWithoutParentalCareAdults,
        int? StudentsWithSpecialPsychophysicalDevelopmentNeeds,
        int? StudentsWithDisabledParentsOfGroups1and2,
        int? StudentsFromLargeFamilies,
        int? StudentsFromSingleParentFamilies,
        int? StudentsFromRegionsAffectedByChernobylDisaster,
        int? StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution,
        int? NonResidentStudents,
        int? StudentsLivingWithParents,
        int? StudentsLivingInADormitory,
        int? StudentsLivingWithRelatives,
        int? StudentsLivingInPrivateApartments,
        string? OtherInformation
    );
}
