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
            builder.Property(c => c.PhoneNumber).HasColumnType("nvarchar(19)");

            builder
                .HasOne(c => c.Page)
                .WithMany(p => p.ContactPhoneNumbers)
                .HasForeignKey(c => c.PageId)
                .IsRequired();
        }
    }
}
