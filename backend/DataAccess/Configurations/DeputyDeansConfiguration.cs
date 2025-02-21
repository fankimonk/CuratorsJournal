using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class DeputyDeansConfiguration : IEntityTypeConfiguration<DeputyDean>
    {
        public void Configure(EntityTypeBuilder<DeputyDean> builder)
        {
            builder.HasKey(dd => dd.Id);

            builder
                .HasOne(dd => dd.Worker)
                .WithOne(w => w.DeputyDean)
                .HasForeignKey<DeputyDean>(dd => dd.WorkerId)
                .IsRequired();
        }
    }
}
