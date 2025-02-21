using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class StudentsConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.PhoneNumber).HasColumnType("nvarchar(17)");

            builder
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
