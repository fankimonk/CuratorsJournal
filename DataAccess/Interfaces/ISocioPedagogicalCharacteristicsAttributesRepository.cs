using Domain.Entities.JournalContent.Pages.Attributes;

namespace DataAccess.Interfaces
{
    public interface ISocioPedagogicalCharacteristicsAttributesRepository
    {
        Task<SocioPedagogicalCharacteristicsPageAttributes?> UpdateAcademicYear(int id, int? academicYearId);
    }
}
