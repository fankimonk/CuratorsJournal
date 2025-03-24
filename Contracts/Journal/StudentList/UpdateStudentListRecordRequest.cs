using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.StudentList
{
    public record UpdateStudentListRecordRequest
    (
        [Range(1, int.MaxValue, ErrorMessage = "Номер должен быть больше 0")]
        int? Number,

        int? StudentId,
        int? PersonalizedAccountingCardId
    );
}
