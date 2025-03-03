using Contracts.Journal.StudentList;
using Domain.Entities.JournalContent;

namespace API.Mappers
{
    public static class StudentListRecordsMapper
    {
        public static StudentListRecordResponse ToResponse(this StudentListRecord record)
        {
            return new StudentListRecordResponse(
                record.Id, record.Number, record.StudentId, record.PersonalizedAccountingCardId
            );
        }

        public static StudentListRecord ToEntity(this UpdateStudentListRecordRequest request)
        {
            return new StudentListRecord
            {
                Number = request.Number,
                PersonalizedAccountingCardId = request.PersonalizedAccountingCardId
            };
        }

        public static StudentListRecord ToEntity(this CreateStudentListRecordRequest request)
        {
            return new StudentListRecord
            {
                Number = request.Number,
                StudentId = request.StudentId,
                PersonalizedAccountingCardId = request.PersonalizedAccountingCardId,
                PageId = request.PageId
            };
        }
    }
}
