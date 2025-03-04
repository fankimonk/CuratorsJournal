namespace Domain.Entities.JournalContent.DynamicsOfKeyIndicators
{
    public class KeyIndicator
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<DynamicsOfKeyIndicatorsRecord> DynamicsOfKeyIndicatorsRecords { get; set; } = [];
    }
}
