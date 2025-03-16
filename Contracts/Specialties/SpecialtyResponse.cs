namespace Contracts.Specialties
{
    public class SpecialtyResponse(
        int id, string name, string abbreviatedName, int departmentId)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;
        public string AbbreviatedName { get; set; } = abbreviatedName;

        public int DepartmentId { get; set; } = departmentId;
    }
}
