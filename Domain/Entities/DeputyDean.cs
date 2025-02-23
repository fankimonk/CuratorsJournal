namespace Domain.Entities
{
    public class DeputyDean
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        public Deanery? Deanery { get; set; }
    }
}
