using Contracts.AcademicYears;
using Domain.Entities;

namespace API.Mappers
{
    public static class AcademicYearsMapper
    {
        public static AcademicYearResponse ToResponse(this AcademicYear academicYear)
        {
            return new AcademicYearResponse(academicYear.Id, academicYear.StartYear, academicYear.EndYear);
        }

        public static AcademicYear ToEntity(this CreateAcademicYearRequest request)
        {
            return new AcademicYear
            {
                StartYear = request.StartYear,
                EndYear = request.EndYear
            };
        }

        public static AcademicYear ToEntity(this UpdateAcademicYearRequest request)
        {
            return new AcademicYear
            {
                StartYear = request.StartYear,
                EndYear = request.EndYear
            };
        }
    }
}
