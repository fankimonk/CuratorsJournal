using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.StudentList
{
    public record CreateStudentListRecordRequest
    (
        int? Number,

        int? StudentId,
        int? PersonalizedAccountingCardId,

        [Required]
        int PageId
    );
}
