using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PsychologicalAndPedagogicalCharacteristicsConfiguration : IEntityTypeConfiguration<PsychologicalAndPedagogicalCharacteristics>
    {
        public void Configure(EntityTypeBuilder<PsychologicalAndPedagogicalCharacteristics> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Content).HasColumnType("nvarchar(max)");

            builder
                .HasOne(p => p.Worker)
                .WithMany(w => w.PsychologicalAndPedagogicalCharacteristics)
                .HasForeignKey(p => p.WorkerId);

            builder
                .HasOne(p => p.Page)
                .WithOne(p => p.PsychologicalAndPedagogicalCharacteristics)
                .HasForeignKey<PsychologicalAndPedagogicalCharacteristics>(p => p.PageId)
                .IsRequired();
        }
    }
}
