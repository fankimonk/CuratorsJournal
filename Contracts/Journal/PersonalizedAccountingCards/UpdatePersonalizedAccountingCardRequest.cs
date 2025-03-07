namespace Contracts.Journal.PersonalizedAccountingCards
{
    public record UpdatePersonalizedAccountingCardRequest
    (
        DateOnly? BirthDate,

        string? PassportData,
        string? Citizenship,
        string? GraduatedEducationalInstitution,
        string? ResidentialAddress,

        int? StudentId
    );
}
