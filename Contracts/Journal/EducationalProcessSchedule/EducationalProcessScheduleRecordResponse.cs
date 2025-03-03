namespace Contracts.Journal.EducationalProcessSchedule
{
    public class EducationalProcessScheduleRecordResponse(
        int id, int semesterNumber,
        DateOnly startDate, DateOnly endDate,
        DateOnly sessionStartDate, DateOnly sessionEndDate,
        DateOnly? practiceStartDate, DateOnly? practiceEndDate,
        DateOnly vacationStartDate, DateOnly vacationEndDate)
    {
        public int Id { get; set; } = id;

        public int SemesterNumber { get; set; } = semesterNumber;

        public DateOnly StartDate { get; set; } = startDate;
        public DateOnly EndDate { get; set; } = endDate;
        public DateOnly SessionStartDate { get; set; } = sessionStartDate;
        public DateOnly SessionEndDate { get; set; } = sessionEndDate;
        public DateOnly? PracticeStartDate { get; set; } = practiceStartDate;
        public DateOnly? PracticeEndDate { get; set; } = practiceEndDate;
        public DateOnly VacationStartDate { get; set; } = vacationStartDate;
        public DateOnly VacationEndDate { get; set; } = vacationEndDate;
    }
}
