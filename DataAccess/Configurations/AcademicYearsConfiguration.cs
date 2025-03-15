using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class AcademicYearsConfiguration : IEntityTypeConfiguration<AcademicYear>
    {
        public void Configure(EntityTypeBuilder<AcademicYear> builder)
        {
            builder.HasKey(ay => ay.Id);

            builder.ToTable(t => t.HasCheckConstraint("CHK_AY_StartEndYear)", "[EndYear] - [StartYear] = 1"));
        }
    }
}
