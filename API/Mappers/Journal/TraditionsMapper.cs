using Contracts.Journal.ContactPhones;
using Contracts.Journal.Traditions;
using Domain.Entities.JournalContent;

namespace API.Mappers.Journal
{
    public static class TraditionsMapper
    {
        public static TraditionResponse ToResponse(this Tradition tradition)
        {
            return new TraditionResponse(
                tradition.Id, tradition.Name, tradition.ParticipationForm, tradition.Note, tradition.Day, tradition.Month
            );
        }

        public static Tradition ToEntity(this UpdateTraditionRequest request)
        {
            return new Tradition
            {
                Name = request.Name,
                ParticipationForm = request.ParticipationForm,
                Note = request.Note,
                Day = request.Day,
                Month = request.Month
            };
        }

        public static Tradition ToEntity(this CreateTraditionRequest request)
        {
            return new Tradition
            {
                Name = request.Name,
                ParticipationForm = request.ParticipationForm,
                Note = request.Note,
                Day = request.Day,
                Month = request.Month,
                PageId = request.PageId
            };
        }
    }
}
