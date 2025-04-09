namespace Contracts.Students
{
    public class StudentResponse(
        int id, string firstName, string middleName,
        string lastName, string? phoneNumber, int groupId)
    {
        public int Id { get; set; } = id;

        public string FirstName { get; set; } = firstName;
        public string MiddleName { get; set; } = middleName;
        public string LastName { get; set; } = lastName;

        public string? PhoneNumber { get; set; } = phoneNumber;

        public int GroupId { get; set; } = groupId;
    }
}
