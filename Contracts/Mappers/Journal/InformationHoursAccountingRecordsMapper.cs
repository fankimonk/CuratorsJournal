using Contracts.Journal.InformationHoursAccounting;

namespace Contracts.Mappers.Journal
{
    public static class InformationHoursAccountingRecordsMapper
    {
        public static UpdateInformationHoursAccountingRecordRequest ToRequest(this InformationHoursAccountingRecordResponse tradition)
        {
            return new UpdateInformationHoursAccountingRecordRequest(
                tradition.Date, tradition.Topic, tradition.Note
            );
        }
    }
}
