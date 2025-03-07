namespace Contracts.Journal.SocioPedagogicalCharacteristics
{
    public class SocioPedagogicalCharacteristicsResponse(
        int id, int? totalStudents, int? femalesCount, int? malesCount,
        int? brsmMembersCount, int? orphansUnderagesCount,
        int? studentsWithoutParentalCareUnderagesCount,
        int? orphansOrStudentsWithoutParentalCareAdults,
        int? studentsWithSpecialPsychophysicalDevelopmentNeeds,
        int? studentsWithDisabledParentsOfGroups1and2,
        int? studentsFromLargeFamilies,
        int? studentsFromSingleParentFamilies,
        int? studentsFromRegionsAffectedByChernobylDisaster,
        int? studentsFromFamiliesRelocatedFromAreasOfRadioactivePollution,
        int? nonResidentStudents, int? studentsLivingWithParents,
        int? studentsLivingInADormitory, int? studentsLivingWithRelatives,
        int? studentsLivingInPrivateApartments, string? otherInformation,
        SocioPedagogicalCharacteristicsAttributesResponse attributes, int pageId)
    {
        public int Id { get; set; } = id;

        public int? TotalStudents { get; set; } = totalStudents;
        public int? FemalesCount { get; set; } = femalesCount;
        public int? MalesCount { get; set; } = malesCount;
        public int? BRSMMembersCount { get; set; } = brsmMembersCount;
        public int? OrphansUnderagesCount { get; set; } = orphansUnderagesCount;
        public int? StudentsWithoutParentalCareUnderagesCount { get; set; } = studentsWithoutParentalCareUnderagesCount;
        public int? OrphansOrStudentsWithoutParentalCareAdults { get; set; } = orphansOrStudentsWithoutParentalCareAdults;
        public int? StudentsWithSpecialPsychophysicalDevelopmentNeeds { get; set; } = studentsWithSpecialPsychophysicalDevelopmentNeeds;
        public int? StudentsWithDisabledParentsOfGroups1and2 { get; set; } = studentsWithDisabledParentsOfGroups1and2;
        public int? StudentsFromLargeFamilies { get; set; } = studentsFromLargeFamilies;
        public int? StudentsFromSingleParentFamilies { get; set; } = studentsFromSingleParentFamilies;
        public int? StudentsFromRegionsAffectedByChernobylDisaster { get; set; } = studentsFromRegionsAffectedByChernobylDisaster;
        public int? StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution { get; set; } = studentsFromFamiliesRelocatedFromAreasOfRadioactivePollution;
        public int? NonResidentStudents { get; set; } = nonResidentStudents;
        public int? StudentsLivingWithParents { get; set; } = studentsLivingWithParents;
        public int? StudentsLivingInADormitory { get; set; } = studentsLivingInADormitory;
        public int? StudentsLivingWithRelatives { get; set; } = studentsLivingWithRelatives;
        public int? StudentsLivingInPrivateApartments { get; set; } = studentsLivingInPrivateApartments;
        public string? OtherInformation { get; set; } = otherInformation;

        public SocioPedagogicalCharacteristicsAttributesResponse Attributes { get; set; } = attributes;

        public int PageId { get; set; } = pageId;
    }
}
