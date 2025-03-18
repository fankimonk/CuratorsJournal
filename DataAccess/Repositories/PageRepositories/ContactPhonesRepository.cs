using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class ContactPhonesRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IContactPhonesRepository
    {
        public async Task<ContactPhoneNumber?> CreateAsync(ContactPhoneNumber phone)
        {
            if (phone == null) return null;
            if (!await PageExists(phone.PageId)) return null;

            var createdPhone = await _dbContext.ContactPhoneNumbers.AddAsync(phone);

            await _dbContext.SaveChangesAsync();

            return createdPhone.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.ContactPhoneNumbers.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ContactPhoneNumber>?> GetByPageIdAsync(int id)
        {
            if (!await PageExists(id)) return null;
            return await _dbContext.ContactPhoneNumbers.AsNoTracking().Where(c => c.PageId == id).ToListAsync();
        }

        public async Task<ContactPhoneNumber?> UpdateAsync(int id, ContactPhoneNumber phone)
        {
            if (phone == null) return null;

            var phoneToUpdate = await _dbContext.ContactPhoneNumbers.FirstOrDefaultAsync(p => p.Id == id);
            if (phoneToUpdate == null) return null;

            phoneToUpdate.Name = phone.Name;
            phoneToUpdate.PhoneNumber = phone.PhoneNumber;

            await _dbContext.SaveChangesAsync();
            return phoneToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.ContactPhones);
    }
}
