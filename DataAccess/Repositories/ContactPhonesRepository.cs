using DataAccess.Interfaces;
using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ContactPhonesRepository : IContactPhonesRepository
    {
        private readonly CuratorsJournalDBContext _dbContext;

        public ContactPhonesRepository(CuratorsJournalDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ContactPhoneNumber?> CreateAsync(ContactPhoneNumber phone)
        {
            if (phone == null) return null;

            var createdPhone = await _dbContext.ContactPhoneNumbers.AddAsync(phone);
            if (createdPhone == null) return null;

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

        public async Task<List<ContactPhoneNumber>> GetByJournalIdAsync(int id)
        {
            return await _dbContext.ContactPhoneNumbers.AsNoTracking().Where(c => c.JournalId == id).ToListAsync();
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
    }
}
