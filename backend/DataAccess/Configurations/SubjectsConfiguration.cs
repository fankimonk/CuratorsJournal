using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class SubjectsConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(s => s.Id);
        }
    }
}
