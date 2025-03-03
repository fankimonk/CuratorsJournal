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

            builder.ToTable(t => t.HasCheckConstraint("CHK_StudentList_Number", "[Number] >= 0"));

            builder
                .HasOne(sl => sl.Student)
                .WithOne(s => s.StudentListRecord)
                .HasForeignKey<StudentListRecord>(sl => sl.StudentId)
                .IsRequired();   

            builder
                .HasOne(sl => sl.PersonalizedAccountingCard)
                .WithMany(pac => pac.StudentList)
                .HasForeignKey(sl => sl.PersonalizedAccountingCardId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(sl => sl.Page)
                .WithMany(p => p.StudentList)
                .HasForeignKey(sl => sl.PageId)
                .IsRequired();
        }
    }
}
