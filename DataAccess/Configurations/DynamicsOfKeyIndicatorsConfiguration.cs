using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class DynamicsOfKeyIndicatorsConfiguration : IEntityTypeConfiguration<DynamicsOfKeyIndicatorsRecord>
    {
        public void Configure(EntityTypeBuilder<DynamicsOfKeyIndicatorsRecord> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Note).HasColumnType("nvarchar(max)");

            builder
                .HasOne(d => d.KeyIndicator)
                .WithMany(k => k.DynamicsOfKeyIndicatorsRecords)
                .HasForeignKey(d => d.KeyIndicatorId)
                .IsRequired();

            builder
                .HasOne(d => d.Page)
                .WithMany(p => p.DynamicsOfKeyIndicators)
                .HasForeignKey(d => d.PageId)
                .IsRequired();
        }
    }
}
