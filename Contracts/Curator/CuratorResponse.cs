namespace Contracts.Curator
{
    public class CuratorResponse(int id, string firstName, string middleName, string lastName)
    {
        public int Id { get; set; } = id;
        public string FirstName { get; set; } = firstName;
        public string MiddleName { get; set; } = middleName;
        public string LastName { get; set; } = lastName;
    };
}
