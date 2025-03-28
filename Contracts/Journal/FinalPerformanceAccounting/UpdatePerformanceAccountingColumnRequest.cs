namespace Contracts.Journal.FinalPerformanceAccounting
{
    public class UpdatePerformanceAccountingColumnRequest(int? subjectId)
    {
        public int? SubjectId { get; set; } = subjectId;
    }
}
