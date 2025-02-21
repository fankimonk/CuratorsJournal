using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class FacultiesConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            builder.HasKey(f => f.Id);
        }
    }
}
