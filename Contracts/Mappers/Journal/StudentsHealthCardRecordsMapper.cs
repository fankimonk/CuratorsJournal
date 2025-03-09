using Contracts.Journal.StudentHealthCards;

namespace Contracts.Mappers.Journal
{
    public static class StudentsHealthCardRecordsMapper
    {
        public static UpdateHealthCardRecordRequest ToRequest(this HealthCardRecordResponse record)
        {
            return new UpdateHealthCardRecordRequest(
                record.Number,
                record.MissedClasses,
                record.Note,
                record.StudentId
            );
        }
    }
}
