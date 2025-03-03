using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.StudentList
{
    public record CreateStudentListRecordRequest
    (
        [Required]
        int Number,

        [Required]
        int StudentId,

        int? PersonalizedAccountingCardId,

        [Required]
        int PageId
    );
}
