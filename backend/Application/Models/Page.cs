namespace Application.Models
{
    public class Page
    {
        public string Title { get; protected set; }

        public Page(string title)
        {
            Title = title;
        }
    }
}
