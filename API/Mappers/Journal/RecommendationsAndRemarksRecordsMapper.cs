using Contracts.Journal.LiteratureWork;
using Contracts.Journal.RecommendationsAndRemarks;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Literature;

namespace API.Mappers.Journal
{
    public static class RecommendationsAndRemarksRecordsMapper
    {
        public static RecommendationsAndRemarksRecordResponse ToResponse(this RecomendationsAndRemarksRecord record)
        {
            return new RecommendationsAndRemarksRecordResponse(
                record.Id, record.Date, record.ExecutionDate, record.Content, record.Result, record.ReviewerId
            );
        }

        public static RecomendationsAndRemarksRecord ToEntity(this UpdateRecommendationsAndRemarksRecordRequest request)
        {
            return new RecomendationsAndRemarksRecord
            {
                Date = request.Date,
                ExecutionDate = request.ExecutionDate,
                Content = request.Content,
                Result = request.Result,
                ReviewerId = request.ReviewerId
            };
        }

        public static RecomendationsAndRemarksRecord ToEntity(this CreateRecommendationsAndRemarksRecordRequest request)
        {
            return new RecomendationsAndRemarksRecord
            {
                Date = request.Date,
                ExecutionDate = request.ExecutionDate,
                Content = request.Content,
                Result = request.Result,
                ReviewerId = request.ReviewerId,
                PageId = request.PageId
            };
        }
    }
}
