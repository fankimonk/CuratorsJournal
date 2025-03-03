using Domain.Entities.JournalContent.Pages;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Domain.Entities
{
    public class Journal
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public List<Page> Pages { get; set; } = [];
    }
}
