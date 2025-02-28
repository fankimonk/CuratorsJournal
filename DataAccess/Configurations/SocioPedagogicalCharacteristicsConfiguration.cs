using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class SocioPedagogicalCharacteristicsConfiguration : IEntityTypeConfiguration<SocioPedagogicalCharacteristics>
    {
        public void Configure(EntityTypeBuilder<SocioPedagogicalCharacteristics> builder)
        {
            builder.HasKey(spc => spc.Id);

            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_TotalStudents", "[TotalStudents] >= 0"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_FemalesCount", "[FemalesCount] >= 0 and [FemalesCount] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_MalesCount", "[MalesCount] >= 0 and [MalesCount] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_MalesFemalesSum", "[MalesCount] + [FemalesCount] = [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_BRSMMembers", "[BRSMMembersCount] >= 0 and [BRSMMembersCount] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_OrphansUnderages", "[OrphansUnderagesCount] >= 0 and [OrphansUnderagesCount] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsWithoutParentalCareUnderagesCount", 
                "[StudentsWithoutParentalCareUnderagesCount] >= 0 and [StudentsWithoutParentalCareUnderagesCount] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_OrphansOrStudentsWithoutParentalCareAdults", 
                "[OrphansOrStudentsWithoutParentalCareAdults] >= 0 and [OrphansOrStudentsWithoutParentalCareAdults] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsWithSpecialPsychophysicalDevelopmentNeeds", 
                "[StudentsWithSpecialPsychophysicalDevelopmentNeeds] >= 0 and [StudentsWithSpecialPsychophysicalDevelopmentNeeds] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsWithDisabledParentsOfGroups1and2", 
                "[StudentsWithDisabledParentsOfGroups1and2] >= 0 and [StudentsWithDisabledParentsOfGroups1and2] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsFromLargeFamilies", "[StudentsFromLargeFamilies] >= 0 and [StudentsFromLargeFamilies] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsFromSingleParentFamilies", 
                "[StudentsFromSingleParentFamilies] >= 0 and [StudentsFromSingleParentFamilies] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsFromRegionsAffectedByChernobylDisaster",
                "[StudentsFromRegionsAffectedByChernobylDisaster] >= 0 and [StudentsFromRegionsAffectedByChernobylDisaster] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution",
                "[StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution] >= 0 and [StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_NonResidentStudents", "[NonResidentStudents] >= 0 and [NonResidentStudents] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsLivingWithParents", "[StudentsLivingWithParents] >= 0 and [StudentsLivingWithParents] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsLivingInADormitory", "[StudentsLivingInADormitory] >= 0 and [StudentsLivingInADormitory] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsLivingWithRelatives", 
                "[StudentsLivingWithRelatives] >= 0 and [StudentsLivingWithRelatives] <= [TotalStudents]"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_SPC_StudentsLivingInPrivateApartments", 
                "[StudentsLivingInPrivateApartments] >= 0 and [StudentsLivingInPrivateApartments] <= [TotalStudents]"));

            builder.Property(spc => spc.OtherInformation).HasColumnType("nvarchar(max)");

            builder
                .HasOne(spc => spc.Page)
                .WithMany(p => p.SocioPedagogicalCharacteristics)
                .HasForeignKey(spc => spc.PageId)
                .IsRequired();

            builder
                .HasOne(spc => spc.AcademicYear)
                .WithMany(ay => ay.SocioPedagogicalCharacteristics)
                .HasForeignKey(spc => spc.AcademicYearId)
                .IsRequired();
        }
    }
}
