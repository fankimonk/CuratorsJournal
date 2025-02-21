using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class WorkWithParentsConfiguration : IEntityTypeConfiguration<WorkWithParentsRecord>
    {
        public void Configure(EntityTypeBuilder<WorkWithParentsRecord> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.WorkContent).HasColumnType("nvarchar(max)");
            builder.Property(w => w.Note).HasColumnType("nvarchar(max)");

            builder
                .HasOne(w => w.PersonalizedAccountingCard)
                .WithMany(p => p.WorkWithParents)
                .HasForeignKey(w => w.PersonalizedAccountingCardId)
                .IsRequired();
        }
    }
}
