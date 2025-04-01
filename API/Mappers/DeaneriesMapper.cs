using Contracts.Deaneries;
using Contracts.Faculties;
using Domain.Entities;

namespace API.Mappers
{
    public static class DeaneriesMapper
    {
        public static DeaneryResponse ToResponse(this Deanery position)
        {
            return new DeaneryResponse(
                position.Id, position.FacultyId, position.DeanId, position.DeputyDeanId,
                position.Faculty == null ? null : new FacultyResponse(position.Faculty.Id, position.Faculty.Name, position.Faculty.AbbreviatedName)
            );
        }

        public static Deanery ToEntity(this CreateDeaneryRequest request)
        {
            return new Deanery
            {
                FacultyId = (int)request.FacultyId,
                DeanId = (int)request.DeanId,
                DeputyDeanId = (int)request.DeputyDeanId
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
