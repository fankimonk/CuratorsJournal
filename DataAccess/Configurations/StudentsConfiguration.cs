using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class StudentsConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FirstName).HasColumnType("nvarchar(max)");
            builder.Property(s => s.MiddleName).HasColumnType("nvarchar(max)");
            builder.Property(s => s.LastName).HasColumnType("nvarchar(max)");

            builder.Property(s => s.PhoneNumber).HasColumnType("nvarchar(17)");

            builder
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(s => s.ChronicDiseases)
                .WithMany(cd => cd.Students)
                .UsingEntity<StudentChronicDisease>(
                    l => l.HasOne<ChronicDisease>().WithMany().HasForeignKey(scd => scd.ChronicDiseaseId),
                    r => r.HasOne<Student>().WithMany().HasForeignKey(scd => scd.StudentId)
                );

            builder
                .HasMany(s => s.PEGroups)
                .WithMany(peg => peg.Students)
                .UsingEntity<StudentPEGroup>(
                    l => l.HasOne<PEGroup>().WithMany().HasForeignKey(speg => speg.PEGroupId),
                    r => r.HasOne<Student>().WithMany().HasForeignKey(speg => speg.StudentId)
                );

            builder
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
