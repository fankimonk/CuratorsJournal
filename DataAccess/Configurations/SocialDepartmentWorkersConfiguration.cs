using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class SocialDepartmentWorkersConfiguration : IEntityTypeConfiguration<SocialDepartmentWorker>
    {
        public void Configure(EntityTypeBuilder<SocialDepartmentWorker> builder)
        {
            builder.HasKey(sdw => sdw.Id);

            builder
                .HasOne(sdw => sdw.Worker)
                .WithOne(w => w.SocialDepartmentWorker)
                .HasForeignKey<SocialDepartmentWorker>(sdw => sdw.WorkerId)
                .IsRequired();
        }
    }
}
