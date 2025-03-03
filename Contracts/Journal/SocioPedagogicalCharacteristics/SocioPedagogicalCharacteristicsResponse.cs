namespace Contracts.Journal.SocioPedagogicalCharacteristics
{
    public class SocioPedagogicalCharacteristicsResponse
    {
        public int Id { get; set; }

        public int? TotalStudents { get; set; }
        public int? FemalesCount { get; set; }
        public int? MalesCount { get; set; }
        public int? BRSMMembersCount { get; set; }
        public int? OrphansUnderagesCount { get; set; }
        public int? StudentsWithoutParentalCareUnderagesCount { get; set; }
        public int? OrphansOrStudentsWithoutParentalCareAdults { get; set; }
        public int? StudentsWithSpecialPsychophysicalDevelopmentNeeds { get; set; }
        public int? StudentsWithDisabledParentsOfGroups1and2 { get; set; }
        public int? StudentsFromLargeFamilies { get; set; }
        public int? StudentsFromSingleParentFamilies { get; set; }
        public int? StudentsFromRegionsAffectedByChernobylDisaster { get; set; }
        public int? StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution { get; set; }
        public int? NonResidentStudents { get; set; }
        public int? StudentsLivingWithParents { get; set; }
        public int? StudentsLivingInADormitory { get; set; }
        public int? StudentsLivingWithRelatives { get; set; }
        public int? StudentsLivingInPrivateApartments { get; set; }
        public string? OtherInformation { get; set; } = string.Empty;

        public int PageId { get; set; }
    }
}
