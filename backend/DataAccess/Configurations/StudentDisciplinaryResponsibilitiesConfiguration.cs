using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class StudentDisciplinaryResponsibilitiesConfiguration : IEntityTypeConfiguration<StudentDisciplinaryResponsibility>
    {
        public void Configure(EntityTypeBuilder<StudentDisciplinaryResponsibility> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Misdemeanor).HasColumnType("nvarchar(max)");
            builder.Property(s => s.DisciplinaryResponsibilityKind).HasColumnType("nvarchar(max)");

            builder
                .HasOne(s => s.PersonalizedAccountingCard)
                .WithMany(p => p.StudentDisciplinaryResponsibilities)
                .HasForeignKey(s => s.PersonalizedAccountingCardId)
                .IsRequired();
        }
    }
}
