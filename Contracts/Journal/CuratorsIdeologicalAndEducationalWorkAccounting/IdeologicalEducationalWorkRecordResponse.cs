namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public class IdeologicalEducationalWorkRecordResponse(
        int id, DateOnly? startDate, DateOnly? endDate, string? workContent)
    {
        public int Id { get; set; } = id;

        public DateOnly? StartDate { get; set; } = startDate;
        public DateOnly? EndDate { get; set; } = endDate;

        public string? WorkContent { get; set; } = workContent;
    }
}
