using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PEGroupsConfiguration : IEntityTypeConfiguration<PEGroup>
    {
        public void Configure(EntityTypeBuilder<PEGroup> builder)
        {
            builder.HasKey(peg => peg.Id);

            builder.Property(peg => peg.Name).HasColumnType("nvarchar(max)");
        }
    }
}
