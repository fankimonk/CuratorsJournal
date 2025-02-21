using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ContactPhoneNumbersConfiguration : IEntityTypeConfiguration<ContactPhoneNumber>
    {
        public void Configure(EntityTypeBuilder<ContactPhoneNumber> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).HasColumnType("nvarchar(max)");

            builder
                .HasOne(c => c.Journal)
                .WithMany(j => j.ContactPhoneNumbers)
                .HasForeignKey(c => c.JournalId)
                .IsRequired();
        }
    }
}
