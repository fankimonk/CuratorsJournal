using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class StudentsChronicDiseasesConfiguration : IEntityTypeConfiguration<StudentChronicDisease>
    {
        public void Configure(EntityTypeBuilder<StudentChronicDisease> builder)
        {
            builder.HasKey(scd => new { scd.StudentId, scd.ChronicDiseaseId });
        }
    }
}
