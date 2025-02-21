namespace Domain.Models.JournalContent
{
    public class PersonalizedAccountingCard
    {
        public int Id { get; set; }

        public DateOnly BirthDate { get; set; }

        public string PassportData { get; set; } = string.Empty;
        public string Citizenship { get; set; } = string.Empty;
        public string GraduatedEducationalInstitution { get; set; } = string.Empty;
        public string ResidentialAddress { get; set; } = string.Empty;
        
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int JournalId { get; set; }
        public Journal? Journal { get; set; }
    }
}
