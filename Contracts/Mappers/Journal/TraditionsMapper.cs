using Contracts.Journal.Traditions;

namespace Contracts.Mappers.Journal
{
    public static class TraditionsMapper
    {
        public static UpdateTraditionRequest ToRequest(this TraditionResponse tradition)
        {
            return new UpdateTraditionRequest(
                tradition.Name, tradition.ParticipationForm, tradition.Note, tradition.Day, tradition.Month
            );
        }
    }
}
