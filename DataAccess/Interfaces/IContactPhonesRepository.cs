using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces
{
    public interface IContactPhonesRepository
    {
        Task<List<ContactPhoneNumber>> GetByJournalIdAsync(int id);
        Task<ContactPhoneNumber?> CreateAsync(ContactPhoneNumber phone);
        Task<ContactPhoneNumber?> UpdateAsync(int id, ContactPhoneNumber phone);
        Task<bool> DeleteAsync(int id);
    }
}
