using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface IParticipationInPedagogicalSeminarsRepository : IPageRepositoryBase
    {
        Task<List<CuratorsParticipationInPedagogicalSeminarsRecord>?> GetByPageIdAsync(int pageId);

        Task<CuratorsParticipationInPedagogicalSeminarsRecord?> CreateAsync(
            CuratorsParticipationInPedagogicalSeminarsRecord phone);

        Task<CuratorsParticipationInPedagogicalSeminarsRecord?> UpdateAsync(int id, 
            CuratorsParticipationInPedagogicalSeminarsRecord phone);

        Task<bool> DeleteAsync(int id);
    }
}
