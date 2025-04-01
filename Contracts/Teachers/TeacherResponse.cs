namespace Contracts.Teachers
{
    public class TeacherResponse(int id, string firstName, string middleName, string lastName, int departmentId)
    {
        public int Id { get; set; } = id;

        public string FirstName { get; set; } = firstName;
        public string MiddleName { get; set; } = middleName;
        public string LastName { get; set; } = lastName;

        public int DepartmentId { get; set; } = departmentId;
    }
}
