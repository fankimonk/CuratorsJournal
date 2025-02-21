using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class HeadsOfDepartmentsConfiguration : IEntityTypeConfiguration<HeadOfDepartment>
    {
        public void Configure(EntityTypeBuilder<HeadOfDepartment> builder)
        {
            builder.HasKey(hd => hd.Id);

            builder
                .HasOne(hd => hd.Worker)
                .WithOne(w => w.HeadOfDepartment)
                .HasForeignKey<HeadOfDepartment>(hd => hd.WorkerId)
                .IsRequired();
        }
    }
}
