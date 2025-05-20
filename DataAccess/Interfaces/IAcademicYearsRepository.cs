using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IAcademicYearsRepository
    {
        Task<List<AcademicYear>> GetAllAsync(int? yearSince = null);
        Task<AcademicYear?> CreateAsync(AcademicYear academicYear);
        Task<bool> DeleteAsync(int id);
    }
}
