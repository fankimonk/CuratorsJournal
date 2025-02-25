using Contracts.Journal;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/holiday")]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidaysRepository _holidaysRepository;

        public HolidaysController(IHolidaysRepository holidaysRepository)
        {
            _holidaysRepository = holidaysRepository;
        }

        [HttpGet]
        public async Task<ActionResult<HolidaysPageResponse>> GetAll()
        {
            var holidayTypes = await _holidaysRepository.GetGroupedByTypes();

            var holidayTypeResponses = holidayTypes.Select(ht => new HolidayTypeResponse(
                ht.Id,
                ht.Name,
                ht.Holidays.Select(h => new HolidayResponse(
                    h.Id, h.Day, h.Month, h.RelativeDate, h.Name)).ToList())
            ).ToList();

            return Ok(new HolidaysPageResponse(holidayTypeResponses));
        }
    }
}
