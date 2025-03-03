using Domain.Entities.JournalContent.Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    [Table("EducationalProcessSchedule")]
    public class EducationalProcessScheduleRecord
    {
        public int Id { get; set; }

        public int SemesterNumber { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly SessionStartDate { get; set; }
        public DateOnly SessionEndDate { get; set; }
        public DateOnly? PracticeStartDate { get; set; }
        public DateOnly? PracticeEndDate { get; set; }
        public DateOnly VacationStartDate { get; set; }
        public DateOnly VacationEndDate { get; set; }

        public int PageId { get; set; }
        public Page? Page;
    }
}
