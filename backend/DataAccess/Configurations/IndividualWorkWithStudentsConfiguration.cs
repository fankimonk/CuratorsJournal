using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class IndividualWorkWithStudentsConfiguration : IEntityTypeConfiguration<IndividualWorkWithStudentRecord>
    {
        public void Configure(EntityTypeBuilder<IndividualWorkWithStudentRecord> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.WorkDoneAndRecommendations).HasColumnType("nvarchar(max)");
            builder.Property(i => i.Result).HasColumnType("nvarchar(max)");
    
            builder
                .HasOne(i => i.PersonalizedAccountingCard)
                .WithMany(p => p.IndividualWorkWithStudent)
                .HasForeignKey(i => i.PersonalizedAccountingCardId)
                .IsRequired();
        }
    }
}
