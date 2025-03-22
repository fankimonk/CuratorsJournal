namespace Contracts.Journal.FinalPerformanceAccounting
{
    public record UpdatePerformanceAccountingGradeRequest
    (
        bool? IsPassed,

        int? Grade
    );
}
