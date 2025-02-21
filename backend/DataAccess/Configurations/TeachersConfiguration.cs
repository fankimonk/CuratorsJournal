using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class TeachersConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .HasOne(t => t.Worker)
                .WithOne(t => t.Teacher)
                .HasForeignKey<Teacher>(t => t.WorkerId)
                .IsRequired();

            builder
                .HasOne(t => t.Department)
                .WithMany(d => d.Teachers)
                .HasForeignKey(t => t.DepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
