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
                .HasOne(d => d.Page)
                .WithMany(p => p.DynamicsOfKeyIndicators)
                .HasForeignKey(d => d.PageId)
                .IsRequired();
        }
    }
}
