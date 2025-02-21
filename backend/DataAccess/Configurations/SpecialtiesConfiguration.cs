using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class SpecialtiesConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.HasKey(s => s.Id);

            builder
                .HasOne(s => s.Department)
                .WithMany(d => d.Specialties)
                .HasForeignKey(s => s.DepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
