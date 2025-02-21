using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class CuratorsConfiguration : IEntityTypeConfiguration<Curator>
    {
        public void Configure(EntityTypeBuilder<Curator> builder)
        {
            builder.HasKey(c => c.Id);

            builder
                .HasOne(c => c.Teacher)
                .WithOne(t => t.Curator)
                .HasForeignKey<Curator>(c => c.TeacherId)
                .IsRequired();
        }
    }
}
