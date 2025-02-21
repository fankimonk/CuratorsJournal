using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class JournalsConfiguration : IEntityTypeConfiguration<Journal>
    {
        public void Configure(EntityTypeBuilder<Journal> builder)
        {
            builder.HasKey(j => j.Id);

            builder
                .HasOne(j => j.Group)
                .WithOne(g => g.Journal)
                .HasForeignKey<Journal>(j => j.GroupId)
                .IsRequired();
        }
    }
}
