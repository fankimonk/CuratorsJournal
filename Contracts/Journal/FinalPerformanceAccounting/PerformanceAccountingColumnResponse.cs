using Contracts.Subjects;

namespace Contracts.Journal.FinalPerformanceAccounting
{
    public class PerformanceAccountingColumnResponse(
        int id, int certificationTypeId,
        SubjectResponse? subjectResponse)
    {
        public int Id { get; set; } = id;

        public int CertificationTypeId { get; set; } = certificationTypeId;

        public SubjectResponse? SubjectResponse { get; set; } = subjectResponse;
    }
}
