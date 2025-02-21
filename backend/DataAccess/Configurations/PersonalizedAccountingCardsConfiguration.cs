using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PersonalizedAccountingCardsConfiguration : IEntityTypeConfiguration<PersonalizedAccountingCard>
    {
        public void Configure(EntityTypeBuilder<PersonalizedAccountingCard> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PassportData).HasColumnType("nvarchar(max)");
            builder.Property(p => p.Citizenship).HasColumnType("nvarchar(max)");
            builder.Property(p => p.GraduatedEducationalInstitution).HasColumnType("nvarchar(max)");
            builder.Property(p => p.ResidentialAddress).HasColumnType("nvarchar(max)");

            builder
                .HasOne(p => p.Student)
                .WithOne(s => s.PersonalizedAccountingCard)
                .HasForeignKey<PersonalizedAccountingCard>(p => p.StudentId)
                .IsRequired();

            builder
                .HasOne(p => p.Journal)
                .WithMany(j => j.PersonalizedAccountingCards)
                .HasForeignKey(p => p.JournalId)
                .IsRequired();
        }
    }
}
