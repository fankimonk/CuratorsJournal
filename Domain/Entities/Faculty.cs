namespace Domain.Entities
{
    public class Faculty
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string AbbreviatedName { get; set; } = string.Empty;

        public Deanery? Deanery { get; set; }
    }
}
