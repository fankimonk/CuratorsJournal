namespace Domain.Models
{
    public class Position
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Worker> Workers { get; set; } = [];
    }
}
