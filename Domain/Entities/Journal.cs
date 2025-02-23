using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Domain.Entities
{
    public class Journal
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public List<ContactPhoneNumber> ContactPhoneNumbers { get; set; } = [];
        public List<SocioPedagogicalCharacteristics> SocioPedagogicalCharacteristics { get; set; } = [];
        public List<GroupActive> GroupActives { get; set; } = [];
        public List<PersonalizedAccountingCard> PersonalizedAccountingCards { get; set; } = [];
        public List<StudentsHealthCardRecord> StudentsHealthCards { get; set; } = [];
        public List<FinalPerformanceAccountingRecord> FinalPerformanceAccounting { get; set; } = [];
        public List<CuratorsIdeologicalAndEducationalWorkAccountingRecord> CuratorsIdeologicalAndEducationalWorkAccounting { get; set; } = [];
        public List<DynamicsOfKeyIndicatorsRecord> DynamicsOfKeyIndicators { get; set; } = [];
        public List<InformationHoursAccountingRecord> InformationHoursAccounting { get; set; } = [];
        public List<CuratorsParticipationInPedagogicalSeminars> CuratorsParticipationInPedagogicalSeminars { get; set; } = [];
        public List<LiteratureWorkRecord> LiteratureWork { get; set; } = [];
        public List<RecomendationsAndRemarks> RecomendationsAndRemarks { get; set; } = [];
        public List<Tradition> Traditions { get; set; } = [];
        public List<EducationalProcessScheduleRecord> EducationalProcessSchedule { get; set; } = [];
        public List<PsychologicalAndPedagogicalCharacteristics> PsychologicalAndPedagogicalCharacteristics { get; set; } = [];
    }
}
