namespace Domain.Entities
{
    public class HeadOfDepartment
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        public Department? Department { get; set; }
    }
}
