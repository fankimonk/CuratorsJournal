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

            builder.Property(i => i.Topic).HasColumnType("nvarchar(max)");
            builder.Property(i => i.Note).HasColumnType("nvarchar(max)");

            builder
                .HasOne(i => i.Page)
                .WithMany(p => p.InformationHoursAccounting)
                .HasForeignKey(i => i.PageId)
                .IsRequired();
        }
    }
}
