namespace Contracts.Journal.PsychologicalAndPedagogicalCharacteristics
{
    public class PsychologicalAndPedagogicalCharacteristicsResponse(
        int id, string? content, DateOnly? date, int? workerId, int pageId)
    {
        public int Id { get; set; } = id;

        public string? Content { get; set; } = content;

        public DateOnly? Date { get; set; } = date;

        public int? WorkerId { get; set; } = workerId;
        public int PageId { get; set; } = pageId;
    }
}
