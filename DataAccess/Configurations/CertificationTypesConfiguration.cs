using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class CertificationTypesConfiguration : IEntityTypeConfiguration<CertificationType>
    {
        public void Configure(EntityTypeBuilder<CertificationType> builder)
        {
            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.Name).HasColumnType("nvarchar(max)");
        }
    }
}
