using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class HolidayTypesConfiguration : IEntityTypeConfiguration<HolidayType>
    {
        public void Configure(EntityTypeBuilder<HolidayType> builder)
        {
            builder.HasKey(ht => ht.Id);

            builder.Property(ht => ht.Name).HasColumnType("nvarchar(max)");
        }
    }
}
