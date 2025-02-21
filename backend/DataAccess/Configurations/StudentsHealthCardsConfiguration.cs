using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class StudentsHealthCardsConfiguration : IEntityTypeConfiguration<StudentsHealthCardRecord>
    {
        public void Configure(EntityTypeBuilder<StudentsHealthCardRecord> builder)
        {
            builder.HasKey(hc => hc.Id);

            builder.Property(hc => hc.Note).HasColumnType("nvarchar(max)");

            builder
                .HasOne(hc => hc.Journal)
                .WithMany(j => j.StudentsHealthCards)
                .HasForeignKey(hc => hc.JournalId)
                .IsRequired();

            builder
                .HasOne(hc => hc.Student)
                .WithMany(s => s.StudentsHealthCardRecords)
                .HasForeignKey(hc => hc.StudentId)
                .IsRequired();

            builder
                .HasOne(hc => hc.AcademicYear)
                .WithMany(ay => ay.StudentsHealthCards)
                .HasForeignKey(hc => hc.AcademicYearId)
                .IsRequired();
        }
    }
}
