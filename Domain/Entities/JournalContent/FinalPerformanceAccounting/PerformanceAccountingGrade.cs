namespace Domain.Entities.JournalContent.FinalPerformanceAccounting
{
    public class PerformanceAccountingGrade
    {
        public int Id { get; set; }

        public bool? IsPassed { get; set; }

        public int? Grade { get; set; }

        public int PerformanceAccountingColumnId { get; set; }
        public PerformanceAccountingColumn? PerformanceAccountingColumn { get; set; }

        public int PerformanceAccountingRecordId { get; set; }
        public FinalPerformanceAccountingRecord? PerformanceAccountingRecord { get; set; }
    }
}
