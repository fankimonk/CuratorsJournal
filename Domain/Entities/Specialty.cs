namespace Domain.Entities
{
    public class Specialty
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string AbbreviatedName { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public List<Group> Groups { get; set; } = [];
    }
}
