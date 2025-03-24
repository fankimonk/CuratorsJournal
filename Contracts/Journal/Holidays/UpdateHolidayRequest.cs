using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.Holidays
{
    public record UpdateHolidayRequest
    (
        [Range(1, 31, ErrorMessage = "День должен иметь значение от 1 до 31")]
        int? Day,

        [Range(1, 12, ErrorMessage = "Месяц должен иметь значение от 1 до 12")]
        int? Month,

        string? RelativeDate,
        string? Name,

        bool IsRelativeDate
    );
}
