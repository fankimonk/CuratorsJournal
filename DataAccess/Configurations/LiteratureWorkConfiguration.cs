using Domain.Entities.JournalContent.Literature;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class LiteratureWorkConfiguration : IEntityTypeConfiguration<LiteratureWorkRecord>
    {
        public void Configure(EntityTypeBuilder<LiteratureWorkRecord> builder)
        {
            builder.HasKey(lw => lw.Id);

            builder.Property(lw => lw.ShortAnnotation).HasColumnType("nvarchar(max)");

            builder
                .HasOne(lw => lw.Page)
                .WithMany(p => p.LiteratureWork)
                .HasForeignKey(lw => lw.PageId)
                .IsRequired();

            builder
                .HasOne(lw => lw.Literature)
                .WithMany(l => l.LiteratureWorkRecords)
                .HasForeignKey(lw => lw.LiteratureId);
        }
    }
}
