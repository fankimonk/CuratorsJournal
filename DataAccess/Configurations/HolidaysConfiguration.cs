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

            builder.ToTable(t => t.HasCheckConstraint("CHK_Holidays_Month", "[Month] >= 0 and [Month] <= 12"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_Holidays_Day", "[Day] >= 0 and [Day] <= 31"));

            builder.Property(h => h.Name).HasColumnType("nvarchar(max)");
            builder.Property(h => h.RelativeDate).HasColumnType("nvarchar(max)");

            builder
                .HasOne(h => h.Type)
                .WithMany(t => t.Holidays)
                .HasForeignKey(h => h.TypeId)
                .IsRequired();
        }
    }

    public class HolidayTypesConfiguration : IEntityTypeConfiguration<HolidayType>
    {
        public void Configure(EntityTypeBuilder<HolidayType> builder)
        {
            builder.HasKey(ht => ht.Id);

            builder.Property(ht => ht.Name).HasColumnType("nvarchar(max)");
        }
    }
}
