namespace Domain.Models
{
    public class SocialDepartmentWorker
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }
    }
}
