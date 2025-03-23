using Contracts.Journal.FinalPerformanceAccounting;
using Domain.Entities.JournalContent.FinalPerformanceAccounting;

namespace API.Mappers.Journal.FinalPerformanceAccounting
{
    public static class PerformanceAccountingColumnsMapper
    {
        public static PerformanceAccountingColumnResponse ToResponse(this PerformanceAccountingColumn active)
        {
            return new PerformanceAccountingColumnResponse(
                active.Id, active.CertificationTypeId, active.Subject?.ToResponse()
            );
        }

        public static PerformanceAccountingColumn ToEntity(this UpdatePerformanceAccountingColumnRequest request)
        {
            return new PerformanceAccountingColumn
            {
                SubjectId = request.SubjectId
            };
        }

        public static PerformanceAccountingColumn ToEntity(this CreatePerformanceAccountingColumnRequest request)
        {
            return new PerformanceAccountingColumn
            {
                CertificationTypeId = request.CertificationTypeId,
                SubjectId = request.SubjectId,
                PageId = request.PageId
            };
        }
    }
}
