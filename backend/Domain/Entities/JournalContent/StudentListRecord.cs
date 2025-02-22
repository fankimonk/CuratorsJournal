using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    [Table("StudentList")]
    public class StudentListRecord
    {
        public int Id { get; set; }

        [Range(0, int.MaxValue)]
        public int Number { get; set; }

        public int PersonalizedAccountingCardId { get; set; }
        public PersonalizedAccountingCard? PersonalizedAccountingCard { get; set; }
    }
}
