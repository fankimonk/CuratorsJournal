using Contracts.Journal.InformationHoursAccounting;
using Domain.Entities.JournalContent;

namespace API.Mappers.Journal
{
    public static class InformationHoursAccountingRecordsMapper
    {
        public static InformationHoursAccountingRecordResponse ToResponse(this InformationHoursAccountingRecord record)
        {
            return new InformationHoursAccountingRecordResponse(
                record.Id, record.Date, record.Topic, record.Note
            );
        }

        public static InformationHoursAccountingRecord ToEntity(this UpdateInformationHoursAccountingRecordRequest request)
        {
            return new InformationHoursAccountingRecord
            {
                Date = request.Date,
                Topic = request.Topic,
                Note = request.Note
            };
        }

        public static InformationHoursAccountingRecord ToEntity(this CreateInformationHoursAccountingRecordRequest request)
        {
            return new InformationHoursAccountingRecord
            {
                Date = request.Date,
                Topic = request.Topic,
                Note = request.Note,
                PageId = request.PageId
            };
        }
    }
}
