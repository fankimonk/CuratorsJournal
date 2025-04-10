using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class RolesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IRolesRepository 
    {
        public async Task<List<Role>> GetAllAsync()
        {
            return await _dbContext.Roles.AsNoTracking().ToListAsync();
        }
    }
}
