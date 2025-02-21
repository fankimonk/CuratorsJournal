using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class DeaneriesConfiguration : IEntityTypeConfiguration<Deanery>
    {
        public void Configure(EntityTypeBuilder<Deanery> builder)
        {
            builder.HasKey(d => d.Id);

            builder
                .HasOne(d => d.Faculty)
                .WithOne(f => f.Deanery)
                .HasForeignKey<Deanery>(d => d.FacultyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(d => d.Dean)
                .WithOne(d => d.Deanery)
                .HasForeignKey<Deanery>(d => d.DeanId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(d => d.DeputyDean)
                .WithOne(dd => dd.Deanery)
                .HasForeignKey<Deanery>(d => d.DeputyDeanId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
