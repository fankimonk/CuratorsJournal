using Contracts.CertificationTypes;
using Contracts.Subjects;

namespace Contracts.Journal.FinalPerformanceAccounting
{
    public class PerformanceAccountingColumnResponse(
        int id, CertificationTypeResponse certificationType,
        SubjectResponse? subjectResponse)
    {
        public int Id { get; set; } = id;

        public CertificationTypeResponse CertificationType { get; set; } = certificationType;

        public SubjectResponse? SubjectResponse { get; set; } = subjectResponse;
    }
}
