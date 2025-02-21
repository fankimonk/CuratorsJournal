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

            builder
                .HasOne(e => e.Journal)
                .WithMany(j => j.EducationalProcessSchedule)
                .HasForeignKey(e => e.JournalId)
                .IsRequired();
        }
    }
}
