using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.StudentList
{
    public record UpdateStudentListRecordRequest
    (
        [Required]
        int Number,

        int? PersonalizedAccountingCardId
    );
}
