namespace DataAccess.Interfaces.PageRepositories
{
    public interface IPageRepositoryBase
    {
        Task<bool> PageExists(int id);
    }
}
