using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class TraditionsConfiguration : IEntityTypeConfiguration<Tradition>
    {
        public void Configure(EntityTypeBuilder<Tradition> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).HasColumnType("nvarchar(max)");
            builder.Property(t => t.ParticipationForm).HasColumnType("nvarchar(max)");
            builder.Property(t => t.Note).HasColumnType("nvarchar(max)");

            builder.ToTable(t => t.HasCheckConstraint("CHK_Traditions_Month", "[Month] >= 0 and [Month] <= 12"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_Traditions_Day", "[Day] >= 0 and [Day] <= 31"));

            builder
                .HasOne(t => t.Page)
                .WithMany(p => p.Traditions)
                .HasForeignKey(t => t.PageId)
                .IsRequired();
        }
    }
}
