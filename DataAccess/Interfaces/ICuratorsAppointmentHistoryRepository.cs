using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface ICuratorsAppointmentHistoryRepository
    {
        Task<CuratorsAppointmentHistoryRecord?> CreateAsync(CuratorsAppointmentHistoryRecord record);
        Task<List<CuratorsAppointmentHistoryRecord>> GetAllAsync();
    }
}
