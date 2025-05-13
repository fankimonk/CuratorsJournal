using Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting;

namespace Contracts.Mappers.Journal
{
    public static class IdeologicalEducationalWorkRecordsMapper
    {
        public static UpdateIdeologicalEducationalWorkRecordRequest ToRequest(this IdeologicalEducationalWorkRecordResponse record)
        {
            return new UpdateIdeologicalEducationalWorkRecordRequest(
                record.StartDay, record.EndDay, record.WorkContent
            );
        }
    }
}
