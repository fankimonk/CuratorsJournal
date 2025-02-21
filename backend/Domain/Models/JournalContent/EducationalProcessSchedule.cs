namespace Domain.Models.JournalContent
{
    public class EducationalProcessSchedule
    {
        public int Id { get; set; }

        public int SemesterNumber { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly SessionStartDate { get; set; }
        public DateOnly SessionEndDate { get; set; }
        public DateOnly PracticeStartDate { get; set; }
        public DateOnly PracticeEndDate { get; set; }
        public DateOnly VacationStartDate { get; set; }
        public DateOnly VacationEndDate { get; set; }

        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
