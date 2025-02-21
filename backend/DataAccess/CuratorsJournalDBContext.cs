using DataAccess.Configurations;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CuratorsJournalDBContext(DbContextOptions options) : DbContext(options)
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
