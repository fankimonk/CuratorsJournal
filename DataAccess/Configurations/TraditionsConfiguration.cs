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

            builder
                .HasOne(t => t.Journal)
                .WithMany(j => j.Traditions)
                .HasForeignKey(t => t.JournalId)
                .IsRequired();
        }
    }
}
