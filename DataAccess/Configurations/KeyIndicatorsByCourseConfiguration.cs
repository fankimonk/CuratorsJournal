using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class KeyIndicatorsByCourseConfiguration : IEntityTypeConfiguration<KeyIndicatorByCourse>
    {
        public void Configure(EntityTypeBuilder<KeyIndicatorByCourse> builder)
        {
            builder.HasKey(k => k.Id);

            builder.ToTable(t => t.HasCheckConstraint("CHK_KIBC_Course", "[Course] > 0"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_KIBC_Value", "[Value] >= 0"));

            builder
                .HasOne(k => k.DynamicsRecord)
                .WithMany(d => d.KeyIndicatorsByCourse)
                .HasForeignKey(k => k.DynamicsRecordId)
                .IsRequired();
        }
    }
}
