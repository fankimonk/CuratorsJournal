using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class GroupsConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(g => g.Id);

            //TODO OnDelete

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
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
