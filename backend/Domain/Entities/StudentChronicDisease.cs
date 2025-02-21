namespace Domain.Entities
{
    public class StudentChronicDisease
    {
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int ChronicDiseaseId { get; set; }
        public ChronicDisease? ChronicDisease { get; set; }
    }
}
