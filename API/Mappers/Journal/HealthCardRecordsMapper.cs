using Contracts.Journal.StudentHealthCards;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages.Attributes;

namespace API.Mappers.Journal
{
    public static class HealthCardRecordsMapper
    {
        public static HealthCardRecordResponse ToResponse(this StudentsHealthCardRecord record)
        {
            return new HealthCardRecordResponse(
                record.Id, record.Number, record.MissedClasses, record.Note, record.StudentId
            );
        }

        public static HealthCardPageAttributesResponse ToResponse(this HealthCardPageAttributes attributes)
        {
            return new HealthCardPageAttributesResponse(attributes.Id,
                attributes.AcademicYearId);
        }

        public static StudentsHealthCardRecord ToEntity(this UpdateHealthCardRecordRequest request)
        {
            return new StudentsHealthCardRecord
            {
                Number = request.Number,
                MissedClasses = request.MissedClasses,
                Note = request.Note,
                StudentId = request.StudentId
            };
        }

        public static StudentsHealthCardRecord ToEntity(this CreateHealthCardRecordRequest request)
        {
            return new StudentsHealthCardRecord
            {
                Number = request.Number,
                MissedClasses = request.MissedClasses,
                Note = request.Note,
                StudentId = request.StudentId,
                PageId = request.PageId
            };
        }
    }
}
