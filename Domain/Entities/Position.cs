namespace Domain.Entities
{
    public class Position
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsDefaultPosition { get; set; }

        public List<Worker> Workers { get; set; } = [];
    }
}
