namespace DataAccess.Interfaces
{
    public interface IPageRepositoryBase
    {
        Task<bool> PageExists(int id);
    }
}
