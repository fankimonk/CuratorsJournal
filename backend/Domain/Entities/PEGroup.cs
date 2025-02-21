namespace Domain.Entities
{
    public class PEGroup
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Student> Students { get; set; } = [];
    }
}
