using Domain.Entities.JournalContent.Pages.Attributes;

namespace DataAccess.Interfaces
{
    public interface IIdeologicalAndEducationalWorkPageAttributesRepository
    {
        Task<CuratorsIdeologicalAndEducationalWorkPageAttributes?> UpdateAttributes(int id, int? month, int? year);
        Task<CuratorsIdeologicalAndEducationalWorkPageAttributes?> GetByPageId(int pageId);
    }
}
