using Contracts.Journal.LiteratureWork;

namespace Contracts.Mappers.Journal
{
    public static class LiteratureWorkRecordsMapper
    {
        public static UpdateLiteratureWorkRecordRequest ToRequest(this LiteratureWorkRecordResponse record)
        {
            return new UpdateLiteratureWorkRecordRequest(
                record.LiteratureId,
                record.ShortAnnotation
            );
        }
    }
}
