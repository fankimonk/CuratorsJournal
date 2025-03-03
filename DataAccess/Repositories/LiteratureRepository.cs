using DataAccess.Interfaces;
using Domain.Entities.JournalContent.Literature;

namespace DataAccess.Repositories
{
    public class LiteratureRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), ILiteratureRepository
    {
        public Task<LiteratureListRecord?> CreateAsync(LiteratureListRecord literature)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<LiteratureListRecord>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LiteratureListRecord?> UpdateAsync(int id, LiteratureListRecord literature)
        {
            throw new NotImplementedException();
        }
    }
}
