using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ChronicDiseasesConfiguration : IEntityTypeConfiguration<ChronicDisease>
    {
        public void Configure(EntityTypeBuilder<ChronicDisease> builder)
        {
            builder.HasKey(cd => cd.Id);

            builder.Property(cd => cd.Name).HasColumnType("nvarchar(max)");
        }
    }
}
