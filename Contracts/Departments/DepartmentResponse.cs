namespace Contracts.Departments
{
    public class DepartmentResponse(
        int id, string name, string abbreviatedName, int headId, int deaneryId)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;
        public string AbbreviatedName { get; set; } = abbreviatedName;

        public int HeadId { get; set; } = headId;
        public int DeaneryId { get; set; } = deaneryId;
    }
}
