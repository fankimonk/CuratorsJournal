namespace Contracts.Journal.FinalPerformanceAccounting
{
    public class PerformanceAccountingRecordResponse(
        int id, int? number, int? studentId, List<PerformanceAccountingGradeResponse>? grades)
    {
        public int Id { get; set; } = id;

        public int? Number { get; set; } = number;

        public int? StudentId { get; set; } = studentId;

        public List<PerformanceAccountingGradeResponse>? Grades { get; set; } = grades;
    }
}
