using Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages.Attributes;

namespace API.Mappers.Journal
{
    public static class IdeologicalEducationalWorkMapper
    {
        public static IdeologicalAndEducationalWorkAttributesResponse ToResponse(
            this CuratorsIdeologicalAndEducationalWorkPageAttributes attributes)
        {
            return new IdeologicalAndEducationalWorkAttributesResponse(attributes.Id, attributes.Month, attributes.Year);
        }

        public static IdeologicalEducationalWorkRecordResponse ToResponse(
            this CuratorsIdeologicalAndEducationalWorkAccountingRecord record)
        {
            return new IdeologicalEducationalWorkRecordResponse(
                record.Id, record.StartDay, record.EndDay, record.WorkContent
            );
        }

        public static CuratorsIdeologicalAndEducationalWorkAccountingRecord ToEntity(
            this UpdateIdeologicalEducationalWorkRecordRequest request)
        {
            return new CuratorsIdeologicalAndEducationalWorkAccountingRecord
            {
                StartDay = request.StartDate,
                EndDay = request.EndDate,
                WorkContent = request.WorkContent
            };
        }

        public static CuratorsIdeologicalAndEducationalWorkAccountingRecord ToEntity(
            this CreateIdeologicalEducationalWorkRecordRequest request)
        {
            return new CuratorsIdeologicalAndEducationalWorkAccountingRecord
            {
                StartDay = request.StartDay,
                EndDay = request.EndDay,
                WorkContent = request.WorkContent,
                PageId = request.PageId
            };
        }
    }
}
