using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ParentalInformationConfiguration : IEntityTypeConfiguration<ParentalInformationRecord>
    {
        public void Configure(EntityTypeBuilder<ParentalInformationRecord> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName).HasColumnType("nvarchar(max)");
            builder.Property(p => p.MiddleName).HasColumnType("nvarchar(max)");
            builder.Property(p => p.LastName).HasColumnType("nvarchar(max)");
            builder.Property(p => p.PlaceOfResidence).HasColumnType("nvarchar(max)");
            builder.Property(p => p.PlaceOfWork).HasColumnType("nvarchar(max)");
            builder.Property(p => p.Position).HasColumnType("nvarchar(max)");
            builder.Property(p => p.OtherInformation).HasColumnType("nvarchar(max)");

            builder.Property(p => p.HomePhoneNumber).HasColumnType("nvarchar(19)");
            builder.Property(p => p.WorkPhoneNumber).HasColumnType("nvarchar(19)");
            builder.Property(p => p.MobilePhoneNumber).HasColumnType("nvarchar(19)");
        
            builder
                .HasOne(p => p.PersonalizedAccountingCard)
                .WithMany(p => p.ParentalInformation)
                .HasForeignKey(p => p.PersonalizedAccountingCardId)
                .IsRequired();
        }
    }
}
