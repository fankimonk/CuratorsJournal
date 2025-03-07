using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PersonalizedAccountingCardsConfiguration : IEntityTypeConfiguration<PersonalizedAccountingCard>
    {
        public void Configure(EntityTypeBuilder<PersonalizedAccountingCard> builder)
        {
            builder.HasKey(pac => pac.Id);

            builder.Property(pac => pac.PassportData).HasColumnType("nvarchar(max)");
            builder.Property(pac => pac.Citizenship).HasColumnType("nvarchar(max)");
            builder.Property(pac => pac.GraduatedEducationalInstitution).HasColumnType("nvarchar(max)");
            builder.Property(pac => pac.ResidentialAddress).HasColumnType("nvarchar(max)");

            builder
                .HasOne(pac => pac.Student)
                .WithOne(s => s.PersonalizedAccountingCard)
                .HasForeignKey<PersonalizedAccountingCard>(pac => pac.StudentId);

            builder
                .HasOne(pac => pac.Page)
                .WithOne(p => p.PersonalizedAccountingCard)
                .HasForeignKey<PersonalizedAccountingCard>(pac => pac.PageId)
                .IsRequired();
        }
    }
}
