using Domain.Entities.JournalContent.Literature;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class LiteratureListConfiguration : IEntityTypeConfiguration<LiteratureListRecord>
    {
        public void Configure(EntityTypeBuilder<LiteratureListRecord> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Author).HasColumnType("nvarchar(max)");
            builder.Property(l => l.Name).HasColumnType("nvarchar(max)");
            builder.Property(l => l.BibliographicData).HasColumnType("nvarchar(max)");
        }
    }
}
