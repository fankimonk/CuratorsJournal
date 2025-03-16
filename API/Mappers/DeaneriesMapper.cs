using Contracts.Deaneries;
using Domain.Entities;

namespace API.Mappers
{
    public static class DeaneriesMapper
    {
        public static DeaneryResponse ToResponse(this Deanery position)
        {
            return new DeaneryResponse(
                position.Id, position.FacultyId, position.DeanId, position.DeputyDeanId
            );
        }

        public static Deanery ToEntity(this CreateDeaneryRequest request)
        {
            return new Deanery
            {
                FacultyId = request.FacultyId,
                DeanId = request.DeanId,
                DeputyDeanId = request.DeputyDeanId
            };
        }

        public static Deanery ToEntity(this UpdateDeaneryRequest request)
        {
            return new Deanery
            {
                FacultyId = request.FacultyId,
                DeanId = request.DeanId,
                DeputyDeanId = request.DeputyDeanId
            };
        }
    }
}
