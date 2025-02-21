namespace Domain.Models
{
    public class Faculty
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Deanery? Deanery { get; set; }
    }
}
