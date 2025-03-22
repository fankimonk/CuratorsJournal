using Domain.Entities.JournalContent.Pages;

namespace Domain.Entities.JournalContent.FinalPerformanceAccounting
{
    public class PerformanceAccountingColumn
    {
        public int Id { get; set; }

        public int CertificationTypeId { get; set; }
        public CertificationType? CertificationType { get; set; }

        public int? SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public int PageId { get; set; }
        public Page? Page { get; set; }

        public List<PerformanceAccountingGrade> PerformanceAccountingGrades { get; set; } = [];
    }

}
