namespace Domain.Models
{
    public class Dean
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        public Deanery? Deanery { get; set; }
    }
}
