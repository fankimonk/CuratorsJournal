using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class SocioPedagogicalCharacteristicsConfiguration : IEntityTypeConfiguration<SocioPedagogicalCharacteristics>
    {
        public void Configure(EntityTypeBuilder<SocioPedagogicalCharacteristics> builder)
        {
            builder.HasKey(spc => spc.Id);

            builder.Property(spc => spc.OtherInformation).HasColumnType("nvarchar(max)");

            builder
                .HasOne(spc => spc.Journal)
                .WithMany(j => j.SocioPedagogicalCharacteristics)
                .HasForeignKey(spc => spc.JournalId)
                .IsRequired();

            builder
                .HasOne(spc => spc.AcademicYear)
                .WithMany(ay => ay.SocioPedagogicalCharacteristics)
                .HasForeignKey(spc => spc.AcademicYearId)
                .IsRequired();
        }
    }
}
