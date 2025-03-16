namespace Contracts.Subjects
{
    public class SubjectResponse(
        int id, string name, string abbreviatedName)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;

        public string AbbreviatedName { get; set; } = abbreviatedName;
    }
}
