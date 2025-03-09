using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IAcademicYearsRepository
    {
        Task<List<AcademicYear>> GetAll(int? yearSince = null);
    }
}
