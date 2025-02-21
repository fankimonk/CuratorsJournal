namespace Domain.Entities.JournalContent.PersonalizedAccountingCardContent
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

        public List<ParentalInformationRecord> ParentalInformation { get; set; } = [];
        public List<IndividualInformationRecord> IndividualInformation { get; set; } = [];
        public List<StudentListRecord> StudentList { get; set; } = [];
        public List<StudentEcouragement> StudentEcouragements { get; set; } = [];
        public List<StudentDisciplinaryResponsibility> StudentDisciplinaryResponsibilities { get; set; } = [];
        public List<IndividualWorkWithStudentRecord> IndividualWorkWithStudent { get; set; } = [];
        public List<WorkWithParentsRecord> WorkWithParents { get; set; } = [];
    }
}
