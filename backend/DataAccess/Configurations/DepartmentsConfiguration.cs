using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class DepartmentsConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name).HasColumnType("nvarchar(max)");

            builder
                .HasOne(d => d.HeadOfDepartment)
                .WithOne(hd => hd.Department)
                .HasForeignKey<Department>(d => d.HeadId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(d => d.Deanery)
                .WithMany(d => d.Departments)
                .HasForeignKey(d => d.DeaneryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
