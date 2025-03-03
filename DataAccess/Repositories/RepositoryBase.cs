namespace DataAccess.Repositories
{
    public class RepositoryBase(CuratorsJournalDBContext dbContext)
    {
        protected readonly CuratorsJournalDBContext _dbContext = dbContext;
    }
}
