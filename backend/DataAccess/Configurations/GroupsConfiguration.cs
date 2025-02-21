using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class GroupsConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Number).HasColumnType("nvarchar(8)");

            builder
                .HasOne(g => g.Specialty)
                .WithMany(s => s.Groups)
                .HasForeignKey(g => g.SpecialtyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(g => g.Curator)
                .WithMany(c => c.Groups)
                .HasForeignKey(g => g.CuratorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
