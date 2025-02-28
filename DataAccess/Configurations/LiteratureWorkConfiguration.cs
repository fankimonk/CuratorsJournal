using Domain.Entities.JournalContent;
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
        }
    }
}
