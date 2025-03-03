using Contracts.Journal.LiteratureWork;
using Domain.Entities.JournalContent.Literature;

namespace API.Mappers
{
    public static class LiteratureWorkRecordsMapper
    {
        public static LiteratureWorkRecordResponse ToResponse(this LiteratureWorkRecord record)
        {
            return new LiteratureWorkRecordResponse(
                record.Id, record.LiteratureId, record.ShortAnnotation
            );
        }

        public static LiteratureWorkRecord ToEntity(this UpdateLiteratureWorkRecordRequest request)
        {
            return new LiteratureWorkRecord
            {
                ShortAnnotation = request.ShortAnnotaion
            };
        }

        public static LiteratureWorkRecord ToEntity(this CreateLiteratureWorkRecordRequest request)
        {
            return new LiteratureWorkRecord
            {
                LiteratureId = request.LiteratureId,
                ShortAnnotation = request.ShortAnnotaion,
                PageId = request.PageId
            };
        }
    }
}
