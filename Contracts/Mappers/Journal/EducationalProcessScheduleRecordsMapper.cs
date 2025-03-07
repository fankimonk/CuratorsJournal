using Contracts.Journal.EducationalProcessSchedule;

namespace Contracts.Mappers.Journal
{
    public static class EducationalProcessScheduleRecordsMapper
    {
        public static UpdateEducationalProcessScheduleRecordRequest ToRequest(this EducationalProcessScheduleRecordResponse record)
        {
            return new UpdateEducationalProcessScheduleRecordRequest(
                record.SemesterNumber,
                record.StartDate,
                record.EndDate,
                record.SessionStartDate,
                record.SessionEndDate,
                record.PracticeStartDate,
                record.PracticeEndDate,
                record.VacationStartDate,
                record.VacationEndDate
            );
        }
    }
}
