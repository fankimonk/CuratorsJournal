using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class KeyIndicatorsConfiguration : IEntityTypeConfiguration<KeyIndicator>
    {
        public void Configure(EntityTypeBuilder<KeyIndicator> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(k => k.Name).HasColumnType("nvarchar(max)");

            var keyIndicators = Enum
                .GetValues<KeyIndicators>()
                .Select(ki => new KeyIndicator
                {
                    Id = (int)ki,
                    Name = ki.ToString()
                });

            builder.HasData(keyIndicators);
        }
    }
}
