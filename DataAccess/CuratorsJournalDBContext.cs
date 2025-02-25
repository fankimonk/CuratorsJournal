using DataAccess.Configurations;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DataAccess
{
    public class CuratorsJournalDBContext(DbContextOptions options, IOptions<AuthorizationOptions> authOptions) : DbContext(options)
    {
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Curator> Curators { get; set; }
        public DbSet<Dean> Deans { get; set; }
        public DbSet<Deanery> Deaneries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DeputyDean> DeputyDeans { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<HeadOfDepartment> HeadsOfDepartments { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<SocialDepartmentWorker> SocialDepartmentWorkers { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<ChronicDisease> ChronicDiseases { get; set; }
        public DbSet<PEGroup> PEGroups { get; set; }
        public DbSet<StudentChronicDisease> StudentsChronicDiseases { get; set; }
        public DbSet<StudentPEGroup> StudentsPEGroups { get; set; }
        public DbSet<PersonalizedAccountingCard> PersonalizedAccountingCards { get; set; }
        public DbSet<IndividualInformationRecord> IndividualInformation { get; set; }
        public DbSet<IndividualWorkWithStudentRecord> IndividualWorkWithStudents { get; set; }
        public DbSet<ParentalInformationRecord> ParentalInformation { get; set; }
        public DbSet<StudentDisciplinaryResponsibility> StudentDisciplinaryResponsibilities { get; set; }
        public DbSet<StudentEcouragement> StudentEcouragements { get; set; }
        public DbSet<WorkWithParentsRecord> WorkWithParents { get; set; }
        public DbSet<ContactPhoneNumber> ContactPhoneNumbers { get; set; }
        public DbSet<CuratorsIdeologicalAndEducationalWorkAccountingRecord> CuratorsIdeologicalAndEducationalWorkAccounting { get; set; }
        public DbSet<CuratorsParticipationInPedagogicalSeminars> CuratorsParticipationInPedagogicalSeminars { get; set; }
        public DbSet<DynamicsOfKeyIndicatorsRecord> DynamicsOfKeyIndicators { get; set; }
        public DbSet<EducationalProcessScheduleRecord> EducationalProcessSchedule { get; set; }
        public DbSet<FinalPerformanceAccountingRecord> FinalPerformanceAccounting { get; set; }
        public DbSet<GroupActive> GroupActives { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayType> HolidayTypes { get; set; }
        public DbSet<InformationHoursAccountingRecord> InformationHoursAccounting { get; set; }
        public DbSet<LiteratureWorkRecord> LiteratureWork { get; set; }
        public DbSet<PsychologicalAndPedagogicalCharacteristics> PsychologicalAndPedagogicalCharacteristics { get; set; }
        public DbSet<RecomendationsAndRemarks> RecomendationsAndRemarks { get; set; }
        public DbSet<SocioPedagogicalCharacteristics> SocioPedagogicalCharacteristics { get; set; }
        public DbSet<StudentListRecord> StudentList { get; set; }
        public DbSet<StudentsHealthCardRecord> StudentsHealthCards { get; set; }
        public DbSet<Tradition> Traditions { get; set; }
        public DbSet<CuratorsAppointmentHistoryRecord> CuratorsAppointmentHistory { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolesPermissions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AcademicYearsConfiguration());
            modelBuilder.ApplyConfiguration(new CuratorsConfiguration());
            modelBuilder.ApplyConfiguration(new DeansConfiguration());
            modelBuilder.ApplyConfiguration(new DeaneriesConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentsConfiguration());
            modelBuilder.ApplyConfiguration(new DeputyDeansConfiguration());
            modelBuilder.ApplyConfiguration(new FacultiesConfiguration());
            modelBuilder.ApplyConfiguration(new GroupsConfiguration());
            modelBuilder.ApplyConfiguration(new HeadsOfDepartmentsConfiguration());
            modelBuilder.ApplyConfiguration(new JournalsConfiguration());
            modelBuilder.ApplyConfiguration(new PositionsConfiguration());
            modelBuilder.ApplyConfiguration(new SocialDepartmentWorkersConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialtiesConfiguration());
            modelBuilder.ApplyConfiguration(new StudentsConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectsConfiguration());
            modelBuilder.ApplyConfiguration(new TeachersConfiguration());
            modelBuilder.ApplyConfiguration(new WorkersConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityTypesConfiguration());
            modelBuilder.ApplyConfiguration(new ChronicDiseasesConfiguration());
            modelBuilder.ApplyConfiguration(new PEGroupsConfiguration());
            modelBuilder.ApplyConfiguration(new StudentsChronicDiseasesConfiguration());
            modelBuilder.ApplyConfiguration(new StudentsPEGroupsConfiguration());
            modelBuilder.ApplyConfiguration(new PersonalizedAccountingCardsConfiguration());
            modelBuilder.ApplyConfiguration(new IndividualInformationConfiguration());
            modelBuilder.ApplyConfiguration(new IndividualWorkWithStudentsConfiguration());
            modelBuilder.ApplyConfiguration(new ParentalInformationConfiguration());
            modelBuilder.ApplyConfiguration(new StudentDisciplinaryResponsibilitiesConfiguration());
            modelBuilder.ApplyConfiguration(new StudentEcouragementsConfiguration());
            modelBuilder.ApplyConfiguration(new WorkWithParentsConfiguration());
            modelBuilder.ApplyConfiguration(new ContactPhoneNumbersConfiguration());
            modelBuilder.ApplyConfiguration(new CuratorsIdeologicalAndEducationalWorkAccountingRecordConfiguration());
            modelBuilder.ApplyConfiguration(new CuratorsParticipationInPedagogicalSeminarsConfiguration());
            modelBuilder.ApplyConfiguration(new DynamicsOfKeyIndicatorsConfiguration());
            modelBuilder.ApplyConfiguration(new EducationalProcessScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new FinalPerformanceAccountingConfiguration());
            modelBuilder.ApplyConfiguration(new GroupActivesConfiguration());
            modelBuilder.ApplyConfiguration(new HolidaysConfiguration());
            modelBuilder.ApplyConfiguration(new HolidayTypesConfiguration());
            modelBuilder.ApplyConfiguration(new InformationHoursAccountingConfiguration());
            modelBuilder.ApplyConfiguration(new LiteratureWorkConfiguration());
            modelBuilder.ApplyConfiguration(new PsychologicalAndPedagogicalCharacteristicsConfiguration());
            modelBuilder.ApplyConfiguration(new RecomendationsAndRemarksConfiguration());
            modelBuilder.ApplyConfiguration(new SocioPedagogicalCharacteristicsConfiguration());
            modelBuilder.ApplyConfiguration(new StudentListConfiguration());
            modelBuilder.ApplyConfiguration(new StudentsHealthCardsConfiguration());
            modelBuilder.ApplyConfiguration(new TraditionsConfiguration());
            modelBuilder.ApplyConfiguration(new CuratorsAppointmentHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionsConfiguration());
            modelBuilder.ApplyConfiguration(new RolesConfiguration());
            modelBuilder.ApplyConfiguration(new RolesPermissionsConfiguration(authOptions.Value));
            modelBuilder.ApplyConfiguration(new UsersConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
