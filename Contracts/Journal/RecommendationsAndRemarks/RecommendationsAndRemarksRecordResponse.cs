namespace Contracts.Journal.RecommendationsAndRemarks
{
    public class RecommendationsAndRemarksRecordResponse(
        int id, DateOnly? date, DateOnly? executionDate, string? content, string? result, int? reviewerId)
    {
        public int Id { get; set; } = id;

        public DateOnly? Date { get; set; } = date;
        public DateOnly? ExecutionDate { get; set; } = executionDate;

        public string? Content { get; set; } = content;
        public string? Result { get; set; } = result;

        public int? ReviewerId { get; set; } = reviewerId;
    }
}
