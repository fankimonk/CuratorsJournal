namespace Contracts.Journal.StudentHealthCards
{
    public class HealthCardRecordResponse(
        int id, int number, int missedClasses, string? note, int studentId)
    {
        public int Id { get; set; } = id;

        public int Number { get; set; } = number;
        public int MissedClasses { get; set; } = missedClasses;

        public string? Note { get; set; } = note;

        public int StudentId { get; set; } = studentId;
    }
}
