using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class StudentEcouragementsConfiguration : IEntityTypeConfiguration<StudentEcouragement>
    {
        public void Configure(EntityTypeBuilder<StudentEcouragement> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Achievement).HasColumnType("nvarchar(max)");
            builder.Property(s => s.EncouragementKind).HasColumnType("nvarchar(max)");

            builder
                .HasOne(s => s.PersonalizedAccountingCard)
                .WithMany(p => p.StudentEcouragements)
                .HasForeignKey(s => s.PersonalizedAccountingCardId)
                .IsRequired();
        }
    }
}
