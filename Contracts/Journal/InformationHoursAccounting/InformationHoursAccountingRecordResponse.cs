namespace Contracts.Journal.InformationHoursAccounting
{
    public class InformationHoursAccountingRecordResponse(
        int id, DateOnly? date, string? topic, string? note)
    {
        public int Id { get; set; } = id;

        public DateOnly? Date { get; set; } = date;

        public string? Topic { get; set; } = topic;
        public string? Note { get; set; } = note;
    }
}
