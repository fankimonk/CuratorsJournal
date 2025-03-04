namespace Contracts.Journal.DynamicsOfKeyIndicators
{
    public class KeyIndicatorByCourseResponse(
        int id, int dynamicsRecordId, int course, double value)
    {
        public int Id { get; set; } = id;

        public int DynamicsRecordId { get; set; } = dynamicsRecordId;

        public int Course { get; set; } = course;
        public double Value { get; set; } = value;
    }
}
