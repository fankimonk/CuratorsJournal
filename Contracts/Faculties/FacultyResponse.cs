namespace Contracts.Faculties
{
    public class FacultyResponse
    (
        int id,
        string name,
        string abbreviatedName
    )
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;
        public string AbbreviatedName { get; set; } = abbreviatedName;
    }
}
