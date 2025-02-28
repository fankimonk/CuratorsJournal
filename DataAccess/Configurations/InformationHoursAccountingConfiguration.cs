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
                .HasOne(i => i.Page)
                .WithMany(p => p.InformationHoursAccounting)
                .HasForeignKey(i => i.PageId)
                .IsRequired();
        }
    }
}
