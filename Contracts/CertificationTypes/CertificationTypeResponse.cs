using Contracts.Journal.FinalPerformanceAccounting;

namespace Contracts.CertificationTypes
{
    public class CertificationTypeResponse(
        int id, string name, List<PerformanceAccountingColumnResponse>? performanceAccountingColumns)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;

        public List<PerformanceAccountingColumnResponse>? PerformanceAccountingColumns { get; set; } = performanceAccountingColumns;
    }
}
