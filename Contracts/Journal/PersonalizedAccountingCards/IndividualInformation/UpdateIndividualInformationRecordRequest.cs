namespace Contracts.Journal.PersonalizedAccountingCards.IndividualInformation
{
    public record UpdateIndividualInformationRecordRequest
    (
        string? ActivityName,

        DateOnly? StartDate,
        DateOnly? EndDate,

        string? Result,
        string? Note,

        int? ActivityTypeId
    );
}
