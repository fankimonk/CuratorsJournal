using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class StudentsPEGroupsConfiguration : IEntityTypeConfiguration<StudentPEGroup>
    {
        public void Configure(EntityTypeBuilder<StudentPEGroup> builder)
        {
            builder.HasKey(speg => new { speg.StudentId, speg.PEGroupId });
        }
    }
}
