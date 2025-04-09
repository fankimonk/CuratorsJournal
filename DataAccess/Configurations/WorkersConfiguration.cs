using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class WorkersConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.FirstName).HasColumnType("nvarchar(max)");
            builder.Property(w => w.MiddleName).HasColumnType("nvarchar(max)");
            builder.Property(w => w.LastName).HasColumnType("nvarchar(max)");

            builder
                .HasOne(w => w.Position)
                .WithMany(p => p.Workers)
                .HasForeignKey(w => w.PositionId)
                .IsRequired();
        }
    }
}
