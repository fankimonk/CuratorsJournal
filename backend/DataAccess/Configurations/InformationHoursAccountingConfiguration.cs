using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class InformationHoursAccountingConfiguration : IEntityTypeConfiguration<InformationHoursAccountingRecord>
    {
        public void Configure(EntityTypeBuilder<InformationHoursAccountingRecord> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .HasOne(i => i.Journal)
                .WithMany(j => j.InformationHoursAccounting)
                .HasForeignKey(i => i.JournalId)
                .IsRequired();
        }
    }
}
