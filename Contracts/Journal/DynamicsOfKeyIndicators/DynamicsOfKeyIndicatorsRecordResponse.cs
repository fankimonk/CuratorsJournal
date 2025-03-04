namespace Contracts.Journal.DynamicsOfKeyIndicators
{
    public class DynamicsOfKeyIndicatorsRecordResponse(
        int id, KeyIndicatorResponse keyIndicator, string? note,
        List<KeyIndicatorByCourseResponse> keyIndicatorsByCourse)
    {
        public int Id { get; set; } = id;

        public KeyIndicatorResponse KeyIndicator { get; set; } = keyIndicator;

        public string? Note { get; set; } = note;

        public List<KeyIndicatorByCourseResponse> KeyIndicatorsByCourse { get; set; } = keyIndicatorsByCourse;
    }
}
