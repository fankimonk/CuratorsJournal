namespace Application.Models
{
    public class ContactPhonesPage : Page
    {
        public Dictionary<string, string> ContactPhones { get; private set; }

        public ContactPhonesPage(string title, Dictionary<string, string> contactPhones) : base(title)
        {
            ContactPhones = contactPhones;
        }
    }
}
