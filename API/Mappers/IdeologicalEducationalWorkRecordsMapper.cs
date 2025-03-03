using Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting;
using Domain.Entities.JournalContent;

namespace API.Mappers
{
    public static class IdeologicalEducationalWorkRecordsMapper
    {
        public static IdeologicalEducationalWorkRecordResponse ToResponse(
            this CuratorsIdeologicalAndEducationalWorkAccountingRecord record)
        {
            return new IdeologicalEducationalWorkRecordResponse(
                record.Id, record.StartDate, record.EndDate, record.WorkContent
            );
        }

        public static CuratorsIdeologicalAndEducationalWorkAccountingRecord ToEntity(
            this UpdateIdeologicalEducationalWorkRecordRequest request)
        {
            return new CuratorsIdeologicalAndEducationalWorkAccountingRecord
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                WorkContent = request.WorkContent
            };
        }

        public static CuratorsIdeologicalAndEducationalWorkAccountingRecord ToEntity(
            this CreateIdeologicalEducationalWorkRecordRequest request)
        {
            return new CuratorsIdeologicalAndEducationalWorkAccountingRecord
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                WorkContent = request.WorkContent,
                PageId = request.PageId
            };
        }
    }
}
