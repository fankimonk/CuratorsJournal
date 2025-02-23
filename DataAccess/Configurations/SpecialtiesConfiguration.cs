using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class SpecialtiesConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name).HasColumnType("nvarchar(max)");
            builder.Property(s => s.AbbreviatedName).HasColumnType("nvarchar(max)");

            builder
                .HasOne(s => s.Department)
                .WithMany(d => d.Specialties)
                .HasForeignKey(s => s.DepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
