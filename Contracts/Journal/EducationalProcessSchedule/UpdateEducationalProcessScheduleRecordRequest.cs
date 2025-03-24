using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.EducationalProcessSchedule
{
    public record UpdateEducationalProcessScheduleRecordRequest
    (
        [Range(1, int.MaxValue, ErrorMessage = "Номер семестра должен быть больше 0")]
        int? SemesterNumber,

        DateOnly? StartDate,
        DateOnly? EndDate,
        DateOnly? SessionStartDate,
        DateOnly? SessionEndDate,

        DateOnly? PracticeStartDate,
        DateOnly? PracticeEndDate,

        DateOnly? VacationStartDate,
        DateOnly? VacationEndDate
    );
}
