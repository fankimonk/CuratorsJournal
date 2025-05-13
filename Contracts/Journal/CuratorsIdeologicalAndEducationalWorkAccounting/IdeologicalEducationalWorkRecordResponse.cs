namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public class IdeologicalEducationalWorkRecordResponse(
        int id, int? startDay, int? endDay, string? workContent)
    {
        public int Id { get; set; } = id;

        public int? StartDay { get; set; } = startDay;
        public int? EndDay { get; set; } = endDay;

        public string? WorkContent { get; set; } = workContent;
    }
}
