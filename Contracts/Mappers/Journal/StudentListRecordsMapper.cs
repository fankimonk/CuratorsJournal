using Contracts.Journal.StudentList;

namespace Contracts.Mappers.Journal
{
    public static class StudentListRecordsMapper
    {
        public static UpdateStudentListRecordRequest ToRequest(this StudentListRecordResponse record)
        {
            return new UpdateStudentListRecordRequest(
                record.Number,
                record.StudentId,
                record.CardInfo?.Id
            );
        }
    }
}
