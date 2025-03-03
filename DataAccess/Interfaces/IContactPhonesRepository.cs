using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces
{
    public interface IContactPhonesRepository : IPageRepositoryBase
    {
        Task<List<ContactPhoneNumber>?> GetByPageIdAsync(int pageId);
        Task<ContactPhoneNumber?> CreateAsync(ContactPhoneNumber phone);
        Task<ContactPhoneNumber?> UpdateAsync(int id, ContactPhoneNumber phone);
        Task<bool> DeleteAsync(int id);
    }
}
