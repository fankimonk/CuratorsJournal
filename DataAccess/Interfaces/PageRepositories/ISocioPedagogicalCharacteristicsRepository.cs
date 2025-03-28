using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface ISocioPedagogicalCharacteristicsRepository : IPageRepositoryBase
    {
        Task<SocioPedagogicalCharacteristics?> GetByPageIdAsync(int pageId);
        Task<SocioPedagogicalCharacteristics?> UpdateAsync(int id, SocioPedagogicalCharacteristics characteristics);
    }
}
