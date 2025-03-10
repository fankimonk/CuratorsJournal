namespace Contracts.Journal.StudentList
{
    public record UpdateStudentListRecordRequest
    (
        int? Number,

        int? StudentId,
        int? PersonalizedAccountingCardId
    );
}
