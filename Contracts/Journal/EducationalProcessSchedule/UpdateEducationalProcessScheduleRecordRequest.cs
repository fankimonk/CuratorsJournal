using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.EducationalProcessSchedule
{
    public record UpdateEducationalProcessScheduleRecordRequest
    (
        [Required]
        int SemesterNumber,

        [Required]
        DateOnly StartDate,
        [Required]
        DateOnly EndDate,
        [Required]
        DateOnly SessionStartDate,
        [Required]
        DateOnly SessionEndDate,

        DateOnly? PracticeStartDate,
        DateOnly? PracticeEndDate,

        [Required]
        DateOnly VacationStartDate,
        [Required]
        DateOnly VacationEndDate
    );
}
