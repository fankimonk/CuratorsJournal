namespace Contracts.Journal.InformationHoursAccounting
{
    public record UpdateInformationHoursAccountingRecordRequest
    (
        DateOnly? Date,

        string? Topic,
        string? Note
    );
}
