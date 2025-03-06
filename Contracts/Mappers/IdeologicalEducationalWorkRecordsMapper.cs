using Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting;

namespace Contracts.Mappers
{
    public static class IdeologicalEducationalWorkRecordsMapper
    {
        public static UpdateIdeologicalEducationalWorkRecordRequest ToRequest(this IdeologicalEducationalWorkRecordResponse record)
        {
            return new UpdateIdeologicalEducationalWorkRecordRequest(
                record.StartDate, record.EndDate, record.WorkContent
            );
        }
    }
}
