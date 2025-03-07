using Domain.Entities.JournalContent.Pages.Attributes;

namespace DataAccess.Interfaces
{
    public interface IHealthCardAttributesRepository
    {
        Task<HealthCardPageAttributes?> UpdateAcademicYear(int id, int? academicYearId);
    }
}
