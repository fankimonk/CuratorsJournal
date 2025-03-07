using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class IndividualInformationConfiguration : IEntityTypeConfiguration<IndividualInformationRecord>
    {
        public void Configure(EntityTypeBuilder<IndividualInformationRecord> builder)
        {
            builder.HasKey(i => i.Id);

            builder.ToTable(t => t.HasCheckConstraint("CHK_II_StartEndDates", "[EndDate] >= [StartDate]"));

            builder.Property(i => i.ActivityName).HasColumnType("nvarchar(max)");
            builder.Property(i => i.Result).HasColumnType("nvarchar(max)");
            builder.Property(i => i.Note).HasColumnType("nvarchar(max)");

            builder
                .HasOne(i => i.ActivityType)
                .WithMany(a => a.IndividualInformationRecords)
                .HasForeignKey(i => i.ActivityTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(i => i.PersonalizedAccountingCard)
                .WithMany(p => p.IndividualInformation)
                .HasForeignKey(s => s.PersonalizedAccountingCardId)
                .IsRequired();
        }
    }
}
