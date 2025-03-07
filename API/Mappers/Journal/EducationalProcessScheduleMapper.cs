using Contracts.Journal.EducationalProcessSchedule;
using Domain.Entities.JournalContent;

namespace API.Mappers.Journal
{
    public static class EducationalProcessScheduleMapper
    {
        public static EducationalProcessScheduleRecordResponse ToResponse(this EducationalProcessScheduleRecord record)
        {
            return new EducationalProcessScheduleRecordResponse(
                record.Id,
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

        public static EducationalProcessScheduleRecord ToEntity(this UpdateEducationalProcessScheduleRecordRequest request)
        {
            return new EducationalProcessScheduleRecord
            {
                SemesterNumber = request.SemesterNumber,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                SessionStartDate = request.SessionStartDate,
                SessionEndDate = request.SessionEndDate,
                PracticeStartDate = request.PracticeStartDate,
                PracticeEndDate = request.PracticeEndDate,
                VacationStartDate = request.VacationStartDate,
                VacationEndDate = request.VacationEndDate
            };
        }

        public static EducationalProcessScheduleRecord ToEntity(this CreateEducationalProcessScheduleRecordRequest request)
        {
            return new EducationalProcessScheduleRecord
            {
                SemesterNumber = request.SemesterNumber,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                SessionStartDate = request.SessionStartDate,
                SessionEndDate = request.SessionEndDate,
                PracticeStartDate = request.PracticeStartDate,
                PracticeEndDate = request.PracticeEndDate,
                VacationStartDate = request.VacationStartDate,
                VacationEndDate = request.VacationEndDate,
                PageId = request.PageId
            };
        }
    }
}
