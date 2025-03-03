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

            builder.ToTable(t => t.HasCheckConstraint("CHK_SHC_Number", "[Number] >= 0"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SHC_MissedClasses", "[MissedClasses] >= 0"));

            builder
                .HasOne(hc => hc.Page)
                .WithMany(p => p.StudentsHealthCards)
                .HasForeignKey(hc => hc.PageId)
                .IsRequired();

            builder
                .HasOne(hc => hc.Student)
                .WithMany(s => s.StudentsHealthCardRecords)
                .HasForeignKey(hc => hc.StudentId)
                .IsRequired();
        }
    }
}
