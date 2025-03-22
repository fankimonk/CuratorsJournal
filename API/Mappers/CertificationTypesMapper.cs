using Contracts.CertificationTypes;
using Domain.Entities;

namespace API.Mappers
{
    public static class CertificationTypesMapper
    {
        public static CertificationTypeResponse ToResponse(this CertificationType position)
        {
            return new CertificationTypeResponse(
                position.Id, position.Name
            );
        }

        public static CertificationType ToEntity(this CreateCertificationTypeRequest request)
        {
            return new CertificationType
            {
                Name = request.Name
            };
        }

        public static CertificationType ToEntity(this UpdateCertificationTypeRequest request)
        {
            return new CertificationType
            {
                Name = request.Name
            };
        }
    }
}
