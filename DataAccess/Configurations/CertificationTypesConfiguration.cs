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

            CertificationType[] data = [
                new CertificationType { Id = 1, Name = "Экзамены" },
                new CertificationType { Id = 2, Name = "Зачеты" },
                new CertificationType { Id = 3, Name = "Диф. зачеты" },
                new CertificationType { Id = 4, Name = "КП" },
                new CertificationType { Id = 5, Name = "КР" }
            ];

            builder.HasData(data);
        }
    }
}
