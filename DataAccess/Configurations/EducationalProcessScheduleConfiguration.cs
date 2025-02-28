using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class EducationalProcessScheduleConfiguration : IEntityTypeConfiguration<EducationalProcessScheduleRecord>
    {
        public void Configure(EntityTypeBuilder<EducationalProcessScheduleRecord> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable(t => t.HasCheckConstraint("CHK_EPS_SemesterNumber", "[SemesterNumber] >= 0"));

            builder.ToTable(t => t.HasCheckConstraint("CHK_EPS_StartEndDates", "[EndDate] >= [StartDate]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_EPS_SessionStartEndDates", "[SessionEndDate] >= [SessionStartDate]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_EPS_PracticeStartEndDates", "[PracticeEndDate] >= [PracticeStartDate]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_EPS_VacationStartEndDates", "[VacationEndDate] >= [VacationStartDate]"));

            builder
                .HasOne(e => e.Page)
                .WithMany(p => p.EducationalProcessSchedule)
                .HasForeignKey(e => e.PageId)
                .IsRequired();
        }
    }
}
