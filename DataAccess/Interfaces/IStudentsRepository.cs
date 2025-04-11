using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IStudentsRepository
    {
        Task<List<Student>?> GetAllAsync(int userId, int? groupId);
        Task<List<Student>?> GetByGroupIdAsync(int id);
        Task<List<Student>?> GetByJournalIdAsync(int id);
        Task<Student?> CreateAsync(Student student);
        Task<Student?> UpdateAsync(int id, Student student);
        Task<bool> DeleteAsync(int id);
        Task<List<ChronicDisease>?> GetChronicDiseasesAsync(int studentId);
        Task<List<PEGroup>?> GetPEGroupsAsync(int studentId);
        Task<ChronicDisease?> AddChronicDiseaseAsync(int studentId, int diseaseId);
        Task<bool> DeleteChronicDiseaseAsync(int studentId, int diseaseId);
        Task<PEGroup?> AddPEGroupAsync(int studentId, int peGroupId);
        Task<bool> DeletePEGroupAsync(int studentId, int peGroupId);
    }
}
