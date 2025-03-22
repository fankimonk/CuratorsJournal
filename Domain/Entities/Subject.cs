using Domain.Entities.JournalContent.FinalPerformanceAccounting;

namespace Domain.Entities
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string AbbreviatedName { get; set; } = string.Empty;

        public List<PerformanceAccountingColumn> PerformanceAccountingColumns { get; set; } = [];
    }
}
