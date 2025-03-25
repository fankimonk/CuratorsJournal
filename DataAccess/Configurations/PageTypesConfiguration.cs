using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PageTypesConfiguration : IEntityTypeConfiguration<PageType>
    {
        public void Configure(EntityTypeBuilder<PageType> builder)
        {
            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.Name).HasColumnType("nvarchar(max)");

            builder.ToTable(t => t.HasCheckConstraint("CHK_PageTypes_MaxPages", "[MaxPages] >= 1"));

            var pageTypes = Enum
                .GetValues<PageTypes>()
                .Select(pt => new PageType
                {
                    Id = (int)pt,
                    Name = pt.ToString()
                });

            builder.HasData(pageTypes);
        }
    }
}
