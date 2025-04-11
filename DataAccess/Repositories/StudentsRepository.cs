using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class StudentsRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IStudentsRepository
    {
        public async Task<ChronicDisease?> AddChronicDiseaseAsync(int studentId, int diseaseId)
        {
            if (!await StudentExists(studentId) || !await ChronicDiseaseExists(diseaseId)) return null;

            var created = await _dbContext.StudentsChronicDiseases.AddAsync
                (new StudentChronicDisease { StudentId = studentId, ChronicDiseaseId = diseaseId });

            await _dbContext.SaveChangesAsync();
            return await _dbContext.ChronicDiseases.AsNoTracking().FirstOrDefaultAsync(cd => cd.Id == diseaseId);
        }

        public async Task<PEGroup?> AddPEGroupAsync(int studentId, int peGroupId)
        {
            if (!await StudentExists(studentId) || !await PEGroupExists(peGroupId)) return null;

            var created = await _dbContext.StudentsPEGroups.AddAsync
                (new StudentPEGroup { StudentId = studentId, PEGroupId = peGroupId });

            await _dbContext.SaveChangesAsync();
            return await _dbContext.PEGroups.AsNoTracking().FirstOrDefaultAsync(cd => cd.Id == peGroupId);
        }

        public async Task<Student?> CreateAsync(Student student)
        {
            if (student == null) return null;
            if (!await GroupExists(student.GroupId)) return null;

            var created = await _dbContext.Students.AddAsync(student);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Students.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteChronicDiseaseAsync(int studentId, int diseaseId)
        {
            var deletedRows = await _dbContext.StudentsChronicDiseases
                .Where(sc => sc.StudentId == studentId && sc.ChronicDiseaseId == diseaseId).ExecuteDeleteAsync();

            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePEGroupAsync(int studentId, int peGroupId)
        {
            var deletedRows = await _dbContext.StudentsPEGroups
                .Where(sc => sc.StudentId == studentId && sc.PEGroupId == peGroupId).ExecuteDeleteAsync();

            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>?> GetAllAsync(int userId, int? groupId)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var students = _dbContext.Students.AsNoTracking();

            if (groupId != null)
                students = students.Where(s => s.GroupId == groupId);

            if (user.RoleId == 1 || user.RoleId == 5) return await students.ToListAsync();
            if (user.RoleId == 2 || user.RoleId == 3 || user.RoleId == 4)
            {
                students = students.Include(s => s.Group).ThenInclude(g => g!.Specialty).ThenInclude(s => s!.Department).ThenInclude(d => d!.Deanery);
                if (user.RoleId == 2) return await students.Where(j => j.Group!.Specialty!.Department!.Deanery!.DeanId == user.WorkerId).ToListAsync();
                else if (user.RoleId == 3) return await students.Where(j => j.Group!.Specialty!.Department!.Deanery!.DeputyDeanId == user.WorkerId).ToListAsync();
                else return await students.Where(j => j.Group!.Specialty!.Department!.HeadId == user.WorkerId).ToListAsync();
            }
            if (user.RoleId == 6)
            {
                var teacher = await _dbContext.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.WorkerId == user.WorkerId);
                if (teacher == null) return null;
                return await students.Include(s => s.Group).Where(j => j.Group!.CuratorId == teacher.Id).ToListAsync();
            }

            return null;                
        }

        public async Task<List<Student>?> GetByGroupIdAsync(int id)
        {
            if (!await GroupExists(id)) return null;
            return await _dbContext.Students.AsNoTracking().Where(s => s.GroupId == id).ToListAsync();
        }

        public async Task<List<Student>?> GetByJournalIdAsync(int id)
        {
            if (!await JournalExists(id)) return null;
            return await _dbContext.Students.Include(s => s.Group).ThenInclude(g => g!.Journal).AsNoTracking()
                .Where(s => s.Group!.Journal!.Id == id).ToListAsync();
        }

        public async Task<Student?> UpdateAsync(int id, Student student)
        {
            if (student == null) return null;
            if (!await GroupExists(student.GroupId)) return null;

            var studentToUpdate = await _dbContext.Students.FirstOrDefaultAsync(p => p.Id == id);
            if (studentToUpdate == null) return null;

            studentToUpdate.FirstName = student.FirstName;
            studentToUpdate.MiddleName = student.MiddleName;
            studentToUpdate.LastName = student.LastName;
            studentToUpdate.PhoneNumber = student.PhoneNumber;
            studentToUpdate.GroupId = student.GroupId;

            await _dbContext.SaveChangesAsync();
            return studentToUpdate;
        }

        private async Task<bool> GroupExists(int id) =>
            await _dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id) != null;

        private async Task<bool> JournalExists(int id) =>
            await _dbContext.Journals.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id) != null;

        private async Task<bool> StudentExists(int id) =>
            await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id) != null;

        private async Task<bool> ChronicDiseaseExists(int id) =>
            await _dbContext.ChronicDiseases.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id) != null;

        private async Task<bool> PEGroupExists(int id) =>
            await _dbContext.PEGroups.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id) != null;

        public async Task<List<ChronicDisease>?> GetChronicDiseasesAsync(int studentId)
        {
            var student = await _dbContext.Students.Include(s => s.ChronicDiseases).AsNoTracking().FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null) return null;

            return student.ChronicDiseases.ToList();
        }

        public async Task<List<PEGroup>?> GetPEGroupsAsync(int studentId)
        {
            var student = await _dbContext.Students.Include(s => s.PEGroups).AsNoTracking().FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null) return null;

            return student.PEGroups.ToList();
        }
    }
}
