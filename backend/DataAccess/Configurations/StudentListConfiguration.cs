using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class StudentListConfiguration : IEntityTypeConfiguration<StudentListRecord>
    {
        public void Configure(EntityTypeBuilder<StudentListRecord> builder)
        {
            builder.HasKey(sl => sl.Id);

            builder
                .HasOne(sl => sl.PersonalizedAccountingCard)
                .WithMany(pac => pac.StudentList)
                .HasForeignKey(sl => sl.PersonalizedAccountingCardId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
