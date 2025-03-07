namespace Contracts.Journal.PersonalizedAccountingCards
{
    public class PersonalizedAccountingCardResponse(
        int id, DateOnly? birthDate, string? passportData, string? citizednship,
        string? graduatedEducationalInstitution, string? residentialAddress, int? studentId, int pageId)
    {
        public int Id { get; set; } = id;

        public DateOnly? BirthDate { get; set; } = birthDate;

        public string? PassportData { get; set; } = passportData;
        public string? Citizenship { get; set; } = citizednship;
        public string? GraduatedEducationalInstitution { get; set; } = graduatedEducationalInstitution;
        public string? ResidentialAddress { get; set; } = residentialAddress;

        public int? StudentId { get; set; } = studentId;

        public int PageId { get; set; } = pageId;
    }
}
