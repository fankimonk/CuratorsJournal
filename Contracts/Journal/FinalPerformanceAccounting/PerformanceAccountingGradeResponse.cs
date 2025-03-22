namespace Contracts.Journal.FinalPerformanceAccounting
{
    public class PerformanceAccountingGradeResponse(
        int id, bool? isPassed, int? grade, int columnId)
    {
        public int Id { get; set; } = id;

        public bool? IsPassed { get; set; } = isPassed;

        public int? Grade { get; set; } = grade;

        public int ColumnId { get; set; } = columnId;
    }
}
