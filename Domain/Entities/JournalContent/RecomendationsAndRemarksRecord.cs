using Domain.Entities.JournalContent.Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    [Table("RecommendationsAndRemarks")]
    public class RecomendationsAndRemarksRecord
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }
        public DateOnly ExecutionDate { get; set; } 

        public string Content { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;

        public int ReviewerId { get; set; }
        public Worker? Reviewer { get; set; }

        public int PageId { get; set; }
        public Page? Page;
    }
}
