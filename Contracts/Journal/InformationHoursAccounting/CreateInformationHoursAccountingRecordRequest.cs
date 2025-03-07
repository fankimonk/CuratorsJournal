namespace Contracts.Journal.InformationHoursAccounting
{
    public record CreateInformationHoursAccountingRecordRequest
    (
        DateOnly? Date,

        string? Topic,
        string? Note,
        
        int PageId
    );
}
