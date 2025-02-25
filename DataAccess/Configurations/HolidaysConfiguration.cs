using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class HolidaysConfiguration : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name).HasColumnType("nvarchar(max)");
            builder.Property(h => h.RelativeDate).HasColumnType("nvarchar(max)");

            builder
                .HasOne(h => h.Type)
                .WithMany(t => t.Holidays)
                .HasForeignKey(h => h.TypeId)
                .IsRequired();
        }
    }
}
