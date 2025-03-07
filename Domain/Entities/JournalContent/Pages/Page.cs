using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;
using Domain.Entities.JournalContent.Literature;
using Domain.Entities.JournalContent.Pages.Attributes;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Domain.Entities.JournalContent.Pages
{
    public class Page
    {
        public int Id { get; set; }

        public int PageTypeId { get; set; }
        public PageType? PageType { get; set; }

        public int JournalId { get; set; }
        public Journal? Journal { get; set; }

        public List<ContactPhoneNumber> ContactPhoneNumbers { get; set; } = [];
        public List<CuratorsIdeologicalAndEducationalWorkAccountingRecord> CuratorsIdeologicalAndEducationalWorkAccounting { get; set; } = [];
        public List<CuratorsParticipationInPedagogicalSeminarsRecord> CuratorsParticipationInPedagogicalSeminars { get; set; } = [];
        public List<DynamicsOfKeyIndicatorsRecord> DynamicsOfKeyIndicators { get; set; } = [];
        public List<EducationalProcessScheduleRecord> EducationalProcessSchedule { get; set; } = [];
        public List<FinalPerformanceAccountingRecord> FinalPerformanceAccounting { get; set; } = [];
        public List<GroupActive> GroupActives { get; set; } = [];
        public List<InformationHoursAccountingRecord> InformationHoursAccounting { get; set; } = [];
        public List<LiteratureWorkRecord> LiteratureWork { get; set; } = [];
        public List<RecomendationsAndRemarksRecord> RecomendationsAndRemarks { get; set; } = [];
        public List<StudentsHealthCardRecord> StudentsHealthCards { get; set; } = [];
        public List<Tradition> Traditions { get; set; } = [];
        public List<StudentListRecord> StudentList { get; set; } = [];

        public PersonalizedAccountingCard? PersonalizedAccountingCard { get; set; }
        public PsychologicalAndPedagogicalCharacteristics? PsychologicalAndPedagogicalCharacteristics { get; set; }
        public SocioPedagogicalCharacteristics? SocioPedagogicalCharacteristics { get; set; }

        public HealthCardPageAttributes? HealthCardPageAttributes { get; set; }
        public SocioPedagogicalCharacteristicsPageAttributes? SocioPedagogicalCharacteristicsPageAttributes { get; set; }
        public CuratorsIdeologicalAndEducationalWorkPageAttributes? CuratorsIdeologicalAndEducationalWorkPageAttributes { get; set; }
    }
}
