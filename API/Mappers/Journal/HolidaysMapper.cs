using Contracts.Journal.Holidays;
using Domain.Entities.JournalContent.Holidays;

namespace API.Mappers.Journal
{
    public static class HolidaysMapper
    {
        public static HolidayResponse ToResponse(this Holiday holiday)
        {
            return new HolidayResponse(
                holiday.Id, holiday.Day, holiday.Month, holiday.RelativeDate, holiday.Name, holiday.IsRelativeDate, holiday.TypeId
            );
        }

        public static Holiday ToEntity(this CreateHolidayRequest request)
        {
            return new Holiday
            {
                Day = request.Day,
                Month = request.Month,
                RelativeDate = request.RelativeDate,
                Name = request.Name,
                IsRelativeDate = request.IsRelativeDate,
                TypeId = request.HolidayTypeId,
                PageId = request.PageId
            };
        }

        public static Holiday ToEntity(this UpdateHolidayRequest request)
        {
            return new Holiday
            {
                Day = request.Day,
                Month = request.Month,
                RelativeDate = request.RelativeDate,
                Name = request.Name,
                IsRelativeDate = request.IsRelativeDate
            };
        }

        public static HolidayTypeResponse ToResponse(this HolidayType holidayType)
        {
            return new HolidayTypeResponse(
                holidayType.Id, holidayType.Name, holidayType.Holidays.Select(h => h.ToResponse()).ToList()
            );
        }

        public static HolidayType ToEntity(this CreateHolidayTypeRequest request)
        {
            return new HolidayType
            {
                Name = request.Name
            };
        }

        public static HolidayType ToEntity(this UpdateHolidayTypeRequest request)
        {
            return new HolidayType
            {
                Name = request.Name
            };
        }
    }
}
