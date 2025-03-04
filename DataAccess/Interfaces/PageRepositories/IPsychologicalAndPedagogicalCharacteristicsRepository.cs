using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface IPsychologicalAndPedagogicalCharacteristicsRepository : IPageRepositoryBase
    {
        Task<PsychologicalAndPedagogicalCharacteristics?> GetByPageIdAsync(int pageId);
        Task<PsychologicalAndPedagogicalCharacteristics?> CreateAsync(PsychologicalAndPedagogicalCharacteristics characteristics);
        Task<PsychologicalAndPedagogicalCharacteristics?> UpdateAsync(int id, PsychologicalAndPedagogicalCharacteristics characteristics);
        Task<bool> DeleteAsync(int id);
    }
}
