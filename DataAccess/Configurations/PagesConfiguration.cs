using Domain.Entities.JournalContent.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PagesConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasOne(p => p.PageType)
                .WithMany(pt => pt.Pages)
                .HasForeignKey(p => p.PageTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.Journal)
                .WithMany(j => j.Pages)
                .HasForeignKey(p => p.JournalId)
                .IsRequired();
        }
    }
}
