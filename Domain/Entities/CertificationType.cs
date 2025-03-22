using Domain.Entities.JournalContent.FinalPerformanceAccounting;

namespace Domain.Entities
{
    public class CertificationType
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<PerformanceAccountingColumn> PerformanceAccountingColumns { get; set; } = [];
    }
}
