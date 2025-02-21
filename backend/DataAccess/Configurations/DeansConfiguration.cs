using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class DeansConfiguration : IEntityTypeConfiguration<Dean>
    {
        public void Configure(EntityTypeBuilder<Dean> builder)
        {
            builder.HasKey(d => d.Id);

            builder
                .HasOne(d => d.Worker)
                .WithOne(w => w.Dean)
                .HasForeignKey<Dean>(d => d.WorkerId)
                .IsRequired();
        }
    }
}
