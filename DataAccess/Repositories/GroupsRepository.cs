using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class GroupsRepository(CuratorsJournalDBContext dbContext, 
        ICuratorsAppointmentHistoryRepository curatorsAppointmentHistoryRepository) : RepositoryBase(dbContext), IGroupsRepository
    {
        private readonly ICuratorsAppointmentHistoryRepository _curatorsAppointmentHistoryRepository = curatorsAppointmentHistoryRepository;

        public async Task<List<Group>?> GetAllAsync(int userId)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var groups = _dbContext.Groups.AsNoTracking();

            if (user.RoleId == 1 || user.RoleId == 5) return await groups.ToListAsync();
            if (user.RoleId == 2 || user.RoleId == 3 || user.RoleId == 4)
            {
                groups = groups.Include(g => g.Specialty).ThenInclude(s => s!.Department).ThenInclude(d => d!.Deanery);
                if (user.RoleId == 2) return await groups.Where(j => j.Specialty!.Department!.Deanery!.DeanId == user.WorkerId).ToListAsync();
                else if (user.RoleId == 3) return await groups.Where(j => j.Specialty!.Department!.Deanery!.DeputyDeanId == user.WorkerId).ToListAsync();
                else return await groups.Where(j => j.Specialty!.Department!.HeadId == user.WorkerId).ToListAsync();
            }
            if (user.RoleId == 6)
            {
                var teacher = await _dbContext.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.WorkerId == user.WorkerId);
                if (teacher == null) return null;
                return await groups.Where(j => j.CuratorId == teacher.Id).ToListAsync();
            }

            return null;
        }

        public async Task<Group?> CreateAsync(Group group)
        {
            if (group == null) return null;

            if (group.Number.Length != 8) return null;
            if (!await SpecialtyExists(group.SpecialtyId)) return null;
            if (group.CuratorId != null && !await TeacherExists((int)group.CuratorId)) return null;
            
            var createdGroup = await _dbContext.Groups.AddAsync(group);

            await _dbContext.SaveChangesAsync();

            return createdGroup.Entity;
        }

        public async Task<Group?> UpdateAsync(int id, Group group)
        {
            var groupToUpdate = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if (groupToUpdate == null) return null;

            if (!await SpecialtyExists(group.SpecialtyId)) return null;
            if (group.CuratorId != null)
            {
                if (!await TeacherExists((int)group.CuratorId)) return null;
                if (group.CuratorId != groupToUpdate.CuratorId) await AddCuratorsAppointmentHistoryRecord(groupToUpdate.Id, (int)group.CuratorId);
            }

            groupToUpdate.Number = group.Number;
            groupToUpdate.AdmissionYear = group.AdmissionYear;
            groupToUpdate.CuratorId = group.CuratorId;
            groupToUpdate.SpecialtyId = group.SpecialtyId;

            await _dbContext.SaveChangesAsync();
            return groupToUpdate;
        }

        public async Task<Group?> UpdateCuratorAsync(int id, int? curatorId)
        {
            var groupToUpdate = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if (groupToUpdate == null) return null;

            if (curatorId != null)
            {
                if (!await TeacherExists((int)curatorId)) return null;
                if (curatorId != groupToUpdate.CuratorId) await AddCuratorsAppointmentHistoryRecord(groupToUpdate.Id, (int)curatorId);
            }

            groupToUpdate.CuratorId = curatorId;
            await _dbContext.SaveChangesAsync();
            return groupToUpdate;
        }

        public async Task<Group?> GetByJournalId(int id)
        {
            return await _dbContext.Groups.Include(g => g.Journal).AsNoTracking().FirstOrDefaultAsync(g => g.Journal!.Id == id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Groups.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task<bool> TeacherExists(int id) =>
            await _dbContext.Teachers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> SpecialtyExists(int id) =>
            await _dbContext.Specialties.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<CuratorsAppointmentHistoryRecord?> AddCuratorsAppointmentHistoryRecord(int id, int curatorId)
        {
            var historyRecord = await _curatorsAppointmentHistoryRepository.CreateAsync(
                new CuratorsAppointmentHistoryRecord
                {
                    GroupId = id,
                    CuratorId = curatorId,
                    AppointmentDate = DateOnly.FromDateTime(DateTime.Now)
                });

            if (historyRecord == null) return null;

            return historyRecord;
        }
    }
}
