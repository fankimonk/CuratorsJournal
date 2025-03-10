using Contracts.Journal.RecommendationsAndRemarks;

namespace Contracts.Mappers.Journal
{
    public static class RecommendationsAndRemarksRecordsMapper
    {
        public static UpdateRecommendationsAndRemarksRecordRequest ToRequest(this RecommendationsAndRemarksRecordResponse record)
        {
            return new UpdateRecommendationsAndRemarksRecordRequest(
                record.Date, record.ExecutionDate,
                record.Content, record.Result,
                record.ReviewerId
            );
        }
    }
}
