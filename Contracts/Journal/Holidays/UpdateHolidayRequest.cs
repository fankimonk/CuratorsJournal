using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.Holidays
{
    public record UpdateHolidayRequest
    (
        [Range(1, 31)]
        int? Day,

        [Range(1, 12)]
        int? Month,

        string? RelativeDate,
        string? Name
    );
}
