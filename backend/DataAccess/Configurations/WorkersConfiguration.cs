using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class WorkersConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.HasKey(w => w.Id);

            builder
                .HasOne(w => w.Position)
                .WithMany(p => p.Workers)
                .HasForeignKey(w => w.PositionId)
                .IsRequired();
        }
    }
}
