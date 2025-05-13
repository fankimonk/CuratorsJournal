using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PageTypesConfiguration : IEntityTypeConfiguration<PageType>
    {
        public void Configure(EntityTypeBuilder<PageType> builder)
        {
            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.Name).HasColumnType("nvarchar(max)");

            builder.ToTable(t => t.HasCheckConstraint("CHK_PageTypes_MaxPages", "[MaxPages] >= 1"));

            //var pageTypes = Enum
            //    .GetValues<PageTypes>()
            //    .Select(pt => new PageType
            //    {
            //        Id = (int)pt,
            //        Name = pt.ToString()
            //    });
            var data = new List<PageType>
            {
                new PageType { Id = (int)PageTypes.Title, Name = PageTypes.Title.ToString(), MaxPages = 1 },
                new PageType { Id = (int)PageTypes.ContactPhones, Name = PageTypes.ContactPhones.ToString(), MaxPages = 1 },
                new PageType { Id = (int)PageTypes.Holidays, Name = PageTypes.Holidays.ToString(), MaxPages = 1 },
                new PageType { Id = (int)PageTypes.SocioPedagogicalCharacteristics, Name = PageTypes.SocioPedagogicalCharacteristics.ToString() },
                new PageType { Id = (int)PageTypes.EducationalProcessSchedule, Name = PageTypes.EducationalProcessSchedule.ToString(), MaxPages = 1 },
                new PageType { Id = (int)PageTypes.DynamicsOfKeyIndicators, Name = PageTypes.DynamicsOfKeyIndicators.ToString(), MaxPages = 1 },
                new PageType { Id = (int)PageTypes.GroupActives, Name = PageTypes.GroupActives.ToString() },
                new PageType { Id = (int)PageTypes.StudentList, Name = PageTypes.StudentList.ToString(), MaxPages = 1 },
                new PageType { Id = (int)PageTypes.PersonalizedAccountingCard, Name = PageTypes.PersonalizedAccountingCard.ToString() },
                new PageType { Id = (int)PageTypes.StudentsHealthCard, Name = PageTypes.StudentsHealthCard.ToString() },
                new PageType { Id = (int)PageTypes.FinalPerformanceAccounting, Name = PageTypes.FinalPerformanceAccounting.ToString() },
                new PageType { Id = (int)PageTypes.CuratorsIdeologicalAndEducationalWorkAccounting, Name = PageTypes.CuratorsIdeologicalAndEducationalWorkAccounting.ToString() },
                new PageType { Id = (int)PageTypes.InformationHoursAccounting, Name = PageTypes.InformationHoursAccounting.ToString() },
                new PageType { Id = (int)PageTypes.CuratorsParticipationInPedagogicalSeminars, Name = PageTypes.CuratorsParticipationInPedagogicalSeminars.ToString(), MaxPages = 1 },
                new PageType { Id = (int)PageTypes.LiteratureWork, Name = PageTypes.LiteratureWork.ToString(), MaxPages = 1 },
                new PageType { Id = (int)PageTypes.PsychologicalAndPedagogicalCharacteristics, Name = PageTypes.PsychologicalAndPedagogicalCharacteristics.ToString() },
                new PageType { Id = (int)PageTypes.RecomendationsAndRemarks, Name = PageTypes.RecomendationsAndRemarks.ToString() },
                new PageType { Id = (int)PageTypes.Traditions, Name = PageTypes.Traditions.ToString() },
            };

            builder.HasData(data);
        }
    }
}
