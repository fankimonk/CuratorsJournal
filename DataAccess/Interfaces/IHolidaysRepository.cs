using Domain.Entities.JournalContent.Holidays;

namespace DataAccess.Interfaces
{
    public interface IHolidaysRepository
    {
        Task<List<HolidayType>> GetGroupedByTypes();
        Task<Holiday?> CreateAsync(Holiday holiday);
        Task<Holiday?> UpdateAsync(int id, Holiday holiday);
        Task<bool> DeleteAsync(int id);
        Task<HolidayType?> CreateTypeAsync(HolidayType type);
        Task<HolidayType?> UpdateTypeAsync(int id, HolidayType type);
        Task<bool> DeleteTypeAsync(int id);
    }
}
