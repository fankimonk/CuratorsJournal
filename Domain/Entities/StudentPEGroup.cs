namespace Domain.Entities
{
    public class StudentPEGroup
    {
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int PEGroupId { get; set; }
        public PEGroup? PEGroup { get; set; }
    }
}
