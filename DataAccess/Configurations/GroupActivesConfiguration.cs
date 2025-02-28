using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class GroupActivesConfiguration : IEntityTypeConfiguration<GroupActive>
    {
        public void Configure(EntityTypeBuilder<GroupActive> builder)
        {
            builder.HasKey(ga => ga.Id);

            builder.Property(ga => ga.PositionName).HasColumnType("nvarchar(max)");

            builder
                .HasOne(ga => ga.Student)
                .WithOne(s => s.GroupActive)
                .HasForeignKey<GroupActive>(ga => ga.StudentId)
                .IsRequired();

            builder
                .HasOne(ga => ga.Page)
                .WithMany(p => p.GroupActives)
                .HasForeignKey(ga => ga.PageId)
                .IsRequired();
        }
    }
}
