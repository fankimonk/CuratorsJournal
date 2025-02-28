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
                .HasForeignKey(p => p.WorkerId)
                .IsRequired();

            builder
                .HasOne(p => p.Page)
                .WithMany(p => p.PsychologicalAndPedagogicalCharacteristics)
                .HasForeignKey(p => p.PageId)
                .IsRequired();
        }
    }
}
