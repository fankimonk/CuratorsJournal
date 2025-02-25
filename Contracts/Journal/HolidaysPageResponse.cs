namespace Contracts.Journal
{
    public record HolidaysPageResponse
    (
        List<HolidayTypeResponse> HolidayTypes
    );

    public record HolidayResponse
    (
        int Id,
        int? Day,
        int? Month,
        string? RelativeDate,
        string Name
    );

    public record HolidayTypeResponse
    (
        int Id,
        string Name,
        List<HolidayResponse> Holidays
    );
}
