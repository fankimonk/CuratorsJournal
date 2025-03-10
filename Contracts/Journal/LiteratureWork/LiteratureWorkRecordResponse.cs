namespace Contracts.Journal.LiteratureWork
{
    public class LiteratureWorkRecordResponse(
        int id, int? literatureId, string? shortAnnotation)
    {
        public int Id { get; set; } = id;

        public int? LiteratureId { get; set; } = literatureId;

        public string? ShortAnnotation { get; set; } = shortAnnotation;
    }
}
