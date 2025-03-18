namespace Domain.Entities.JournalContent.DynamicsOfKeyIndicators
{
    public class KeyIndicatorByCourse
    {
        public int Id { get; set; }

        public int DynamicsRecordId { get; set; }
        public DynamicsOfKeyIndicatorsRecord? DynamicsRecord { get; set; }

        public int Course { get; set; }
        public double? Value { get; set; }
    }
}
