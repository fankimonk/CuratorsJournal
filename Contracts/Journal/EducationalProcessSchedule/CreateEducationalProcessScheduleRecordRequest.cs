using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.EducationalProcessSchedule
{
    public record CreateEducationalProcessScheduleRecordRequest
    (
        int? SemesterNumber,

        DateOnly? StartDate,
        DateOnly? EndDate,
        DateOnly? SessionStartDate,
        DateOnly? SessionEndDate,

        DateOnly? PracticeStartDate,
        DateOnly? PracticeEndDate,

        DateOnly? VacationStartDate,
        DateOnly? VacationEndDate,

        [Required]
        int PageId
    );
}
