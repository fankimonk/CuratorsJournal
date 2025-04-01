using Contracts.Specialties;
using Domain.Entities;

namespace API.Mappers
{
    public static class SpecialtiesMapper
    {
        public static SpecialtyResponse ToResponse(this Specialty position)
        {
            return new SpecialtyResponse(
                position.Id, position.Name, position.AbbreviatedName, position.DepartmentId
            );
        }

        public static Specialty ToEntity(this CreateSpecialtyRequest request)
        {
            return new Specialty
            {
                Name = request.Name,
                AbbreviatedName = request.AbbreviatedName,
                DepartmentId = (int)request.DepartmentId
            };
        }

        public static Specialty ToEntity(this UpdateSpecialtyRequest request)
        {
            return new Specialty
            {
                Name = request.Name,
                AbbreviatedName = request.AbbreviatedName,
                DepartmentId = request.DepartmentId
            };
        }
    }
}
