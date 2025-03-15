using Contracts.Faculties;
using Domain.Entities;

namespace API.Mappers
{
    public static class FacultiesMapper
    {
        public static FacultyResponse ToResponse(this Faculty faculty)
        {
            return new FacultyResponse(faculty.Id, faculty.Name, faculty.AbbreviatedName);
        }

        public static Faculty ToEntity(this CreateFacultyRequest request)
        {
            return new Faculty
            {
                Name = request.Name,
                AbbreviatedName = request.AbbreviatedName
            };
        }

        public static Faculty ToEntity(this UpdateFacultyRequest request)
        {
            return new Faculty
            {
                Name = request.Name,
                AbbreviatedName = request.AbbreviatedName
            };
        }
    }
}
