using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class DynamicsOfKeyIndicatorsConfiguration : IEntityTypeConfiguration<DynamicsOfKeyIndicatorsRecord>
    {
        public void Configure(EntityTypeBuilder<DynamicsOfKeyIndicatorsRecord> builder)
        {
            builder.HasKey(d => d.Id);

            builder
                .HasOne(d => d.Journal)
                .WithMany(j => j.DynamicsOfKeyIndicators)
                .HasForeignKey(d => d.JournalId)
                .IsRequired();
        }
    }
}
