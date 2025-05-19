using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class SocioPedagogicalCharacteristicsPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;
        private readonly IGroupsRepository _groupsRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public SocioPedagogicalCharacteristicsPageGenerator(int journalId, Body body, IPagesRepository pagesRepository, IGroupsRepository groupsRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
            _groupsRepository = groupsRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.SocioPedagogicalCharacteristics);
            if (pages == null) throw new ArgumentException(nameof(pages));

            var group = await _groupsRepository.GetByJournalId(_journalId);
            if (group == null) throw new ArgumentException(nameof(group));

            if (page != null)
            {
                GeneratePage(page, group.Number);
            }
            else
            {
                foreach (var p in pages)
                {
                    GeneratePage(p, group.Number);
                }
            }
        }

        private void GeneratePage(Page page, string groupNumber)
        {
            if (page.SocioPedagogicalCharacteristicsPageAttributes == null) throw new ArgumentException(nameof(page));
            if (page.SocioPedagogicalCharacteristics == null) throw new ArgumentException(nameof(page));

            AppendTitle(groupNumber, page.SocioPedagogicalCharacteristicsPageAttributes.AcademicYear);
            WordUtils.AppendBreaks(1, _documentBody);

            AppendContent(page.SocioPedagogicalCharacteristics);

            WordUtils.AppendPageBreak(_documentBody);
        }

        private void AppendTitle(string groupNumber, AcademicYear? academicYear)
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("СОЦИАЛЬНО ПЕДАГОГИЧЕСКАЯ ХАРАКТЕРИСТИКА"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold:true),
                    new Text("учебной группы")),
                new Run(WordUtils.GetRunProperties(underline:true, bold: true),
                    new TabChar(),
                    new Text(groupNumber),
                    new TabChar())
            );

            var startYearPrefixRun = new Run(WordUtils.GetRunProperties(bold: true),
                new Text("в 20"));
            var startYearRun = new Run(WordUtils.GetRunProperties(underline: true, bold: true));

            var endYearPrefixRun = new Run(WordUtils.GetRunProperties(bold: true),
                new Text("/ 20"));
            var endYearRun = new Run(WordUtils.GetRunProperties(underline: true, bold: true));

            if (academicYear != null)
            {
                var startYear = academicYear.StartYear.ToString();
                startYearRun.Append(new Text(startYear.Substring(startYear.Length - 2, 2) + "  "));

                var endYear = academicYear.EndYear.ToString();
                endYearRun.Append(new Text(endYear.Substring(endYear.Length - 2, 2) + "  "));
            }
            else
            {
                startYearRun.Append(new TabChar(), new Text("  "));
                endYearRun.Append(new TabChar(), new Text("   "));
            }

            title.Append(startYearPrefixRun, startYearRun, endYearPrefixRun, endYearRun);

            title.Append(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("уч. году")));

            _documentBody.Append(title);
        }

        private void AppendContent(SocioPedagogicalCharacteristics characteristics)
        {
            var spacing = new SpacingBetweenLines() { Before = "180", After = "180" };
            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }, spacing);

            var totalStudents = characteristics.TotalStudents == null ? "" : ((int)characteristics.TotalStudents).ToString();
            if (totalStudents.Length > 15) totalStudents = totalStudents.Substring(0, 15);
            int tabCount = 3 - (Math.Max(0, totalStudents.Length - 1) / 5);
            var totalStudentsParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("1. Всего студентов")));
            var totalStudentsValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(),
                new Text(totalStudents));
            for (int i = 0; i < tabCount; i++)
                totalStudentsValueRun.Append(new TabChar());
            totalStudentsParagraph.Append(totalStudentsValueRun);
            _documentBody.Append(totalStudentsParagraph);

            _documentBody.Append(new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Start }, spacing.CloneNode(true)),
                new Run(WordUtils.GetRunProperties(),
                    new Text("Из них"))));

            var femalesCount = characteristics.FemalesCount == null ? "" : ((int)characteristics.FemalesCount).ToString();
            if (femalesCount.Length > 20) femalesCount = femalesCount.Substring(0, 20);
            tabCount = 4 - (Math.Max(0, femalesCount.Length - 1) / 5);
            var femalesCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("2. Девушки")));
            var femalesCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(femalesCount));
            for (int i = 0; i < tabCount; i++)
                femalesCountValueRun.Append(new TabChar());
            femalesCountParagraph.Append(femalesCountValueRun);
            _documentBody.Append(femalesCountParagraph);

            var malesCount = characteristics.MalesCount == null ? "" : ((int)characteristics.MalesCount).ToString();
            if (malesCount.Length > 20) malesCount = malesCount.Substring(0, 20);
            tabCount = 4 - (Math.Max(0, malesCount.Length - 1) / 5);
            var malesCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("3. Юноши")));
            var malesCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(malesCount));
            for (int i = 0; i < tabCount; i++)
                malesCountValueRun.Append(new TabChar());
            malesCountParagraph.Append(malesCountValueRun);
            _documentBody.Append(malesCountParagraph);

            var brsmMembersCount = characteristics.BRSMMembersCount == null ? "" : ((int)characteristics.BRSMMembersCount).ToString();
            if (brsmMembersCount.Length > 15) brsmMembersCount = brsmMembersCount.Substring(0, 15);
            tabCount = 3 - (Math.Max(0, brsmMembersCount.Length - 1) / 5);
            var brsmMembersCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("4. Члены ОО \"БРСМ\"")));
            var brsmMembersCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(),
                new Text(brsmMembersCount));
            for (int i = 0; i < tabCount; i++)
                brsmMembersCountValueRun.Append(new TabChar());
            brsmMembersCountParagraph.Append(brsmMembersCountValueRun);
            _documentBody.Append(brsmMembersCountParagraph);

            var orphansUnderagesCount = characteristics.OrphansUnderagesCount == null ? "" : ((int)characteristics.OrphansUnderagesCount).ToString();
            if (orphansUnderagesCount.Length > 10) orphansUnderagesCount = orphansUnderagesCount.Substring(0, 10);
            tabCount = 2 - (Math.Max(0, orphansUnderagesCount.Length - 1) / 5);
            var orphansUnderagesCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("5. Дети-сироты, (до 18 лет)")));
            var orphansUnderagesCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(),
                new Text(orphansUnderagesCount));
            for (int i = 0; i < tabCount; i++)
                orphansUnderagesCountValueRun.Append(new TabChar());
            orphansUnderagesCountParagraph.Append(orphansUnderagesCountValueRun);
            _documentBody.Append(orphansUnderagesCountParagraph);

            var studentsWithoutParentalCareUnderagesCount = 
                characteristics.StudentsWithoutParentalCareUnderagesCount == null ? "" : 
                ((int)characteristics.StudentsWithoutParentalCareUnderagesCount).ToString();
            if (studentsWithoutParentalCareUnderagesCount.Length > 15)
                studentsWithoutParentalCareUnderagesCount = studentsWithoutParentalCareUnderagesCount.Substring(0, 15);
            tabCount = 3 - (Math.Max(0, studentsWithoutParentalCareUnderagesCount.Length - 1) / 5);
            var studentsWithoutParentalCareUnderagesCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("6. Дети, оставшиеся без попечения родителей (до 18 лет)")));
            var studentsWithoutParentalCareUnderagesCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(),
                new Text(studentsWithoutParentalCareUnderagesCount));
            for (int i = 0; i < tabCount; i++)
                studentsWithoutParentalCareUnderagesCountValueRun.Append(new TabChar());
            studentsWithoutParentalCareUnderagesCountParagraph.Append(studentsWithoutParentalCareUnderagesCountValueRun);
            _documentBody.Append(studentsWithoutParentalCareUnderagesCountParagraph);

            _documentBody.Append(new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Start }, spacing.CloneNode(true)),
                new Run(WordUtils.GetRunProperties(),
                    new Text("7. Лица из числа детей-сирот"))));

            var orphansOrStudentsWithoutParentalCareAdultsCount = 
                characteristics.OrphansOrStudentsWithoutParentalCareAdults == null ? "" : 
                ((int)characteristics.OrphansOrStudentsWithoutParentalCareAdults).ToString();
            if (orphansOrStudentsWithoutParentalCareAdultsCount.Length > 10)
                orphansOrStudentsWithoutParentalCareAdultsCount = orphansOrStudentsWithoutParentalCareAdultsCount.Substring(0, 10);
            tabCount = 2 - (Math.Max(0, orphansOrStudentsWithoutParentalCareAdultsCount.Length - 1) / 5);
            var orphansOrStudentsWithoutParentalCareAdultsCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("и детей, оставшихся без попечения родителей (18-23 года)")));
            var orphansOrStudentsWithoutParentalCareAdultsCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(orphansOrStudentsWithoutParentalCareAdultsCount));
            for (int i = 0; i < tabCount; i++)
                orphansOrStudentsWithoutParentalCareAdultsCountValueRun.Append(new TabChar());
            orphansOrStudentsWithoutParentalCareAdultsCountParagraph.Append(orphansOrStudentsWithoutParentalCareAdultsCountValueRun);
            _documentBody.Append(orphansOrStudentsWithoutParentalCareAdultsCountParagraph);

            var studentsWithSpecialPsychophysicalDevelopmentNeedsCount = 
                characteristics.StudentsWithSpecialPsychophysicalDevelopmentNeeds == null ? "" : 
                ((int)characteristics.StudentsWithSpecialPsychophysicalDevelopmentNeeds).ToString();
            if (studentsWithSpecialPsychophysicalDevelopmentNeedsCount.Length > 10)
                studentsWithSpecialPsychophysicalDevelopmentNeedsCount = studentsWithSpecialPsychophysicalDevelopmentNeedsCount.Substring(0, 10);
            tabCount = 2 - (Math.Max(0, studentsWithSpecialPsychophysicalDevelopmentNeedsCount.Length - 1) / 5);
            var studentsWithSpecialPsychophysicalDevelopmentNeedsCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("8. Студенты с особенностями психофизического развития")));
            var studentsWithSpecialPsychophysicalDevelopmentNeedsCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(studentsWithSpecialPsychophysicalDevelopmentNeedsCount));
            for (int i = 0; i < tabCount; i++)
                studentsWithSpecialPsychophysicalDevelopmentNeedsCountValueRun.Append(new TabChar());
            studentsWithSpecialPsychophysicalDevelopmentNeedsCountParagraph.Append(studentsWithSpecialPsychophysicalDevelopmentNeedsCountValueRun);
            _documentBody.Append(studentsWithSpecialPsychophysicalDevelopmentNeedsCountParagraph);

            var studentsWithDisabledParentsOfGroups1and2Count = 
                characteristics.StudentsWithDisabledParentsOfGroups1and2 == null ? "" : 
                ((int)characteristics.StudentsWithDisabledParentsOfGroups1and2).ToString();
            if (studentsWithDisabledParentsOfGroups1and2Count.Length > 20)
                studentsWithDisabledParentsOfGroups1and2Count = studentsWithDisabledParentsOfGroups1and2Count.Substring(0, 20);
            tabCount = 4 - (Math.Max(0, studentsWithDisabledParentsOfGroups1and2Count.Length - 1) / 5);
            var studentsWithDisabledParentsOfGroups1and2CountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("9. Имеющие родителей-инвалидов 1, 2 группы")));
            var studentsWithDisabledParentsOfGroups1and2CountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(studentsWithDisabledParentsOfGroups1and2Count));
            for (int i = 0; i < tabCount; i++)
                studentsWithDisabledParentsOfGroups1and2CountValueRun.Append(new TabChar());
            studentsWithDisabledParentsOfGroups1and2CountParagraph.Append(studentsWithDisabledParentsOfGroups1and2CountValueRun);
            _documentBody.Append(studentsWithDisabledParentsOfGroups1and2CountParagraph);

            var studentsFromLargeFamiliesCount = characteristics.StudentsFromLargeFamilies == null ? "" : ((int)characteristics.StudentsFromLargeFamilies).ToString();
            if (studentsFromLargeFamiliesCount.Length > 35)
                studentsFromLargeFamiliesCount = studentsFromLargeFamiliesCount.Substring(0, 35);
            tabCount = 7 - (Math.Max(0, studentsFromLargeFamiliesCount.Length - 1) / 5);
            var studentsFromLargeFamiliesCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("10. Из многодетных семей")));
            var studentsFromLargeFamiliesCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(studentsFromLargeFamiliesCount));
            for (int i = 0; i < tabCount; i++)
                studentsFromLargeFamiliesCountValueRun.Append(new TabChar());
            studentsFromLargeFamiliesCountParagraph.Append(studentsFromLargeFamiliesCountValueRun);
            _documentBody.Append(studentsFromLargeFamiliesCountParagraph);

            var studentsFromSingleParentFamiliesCount = characteristics.StudentsFromSingleParentFamilies == null ? "" : 
                ((int)characteristics.StudentsFromSingleParentFamilies).ToString();
            if (studentsFromSingleParentFamiliesCount.Length > 40)
                studentsFromSingleParentFamiliesCount = studentsFromSingleParentFamiliesCount.Substring(0, 40);
            tabCount = 8 - (Math.Max(0, studentsFromSingleParentFamiliesCount.Length - 1) / 5);
            var studentsFromSingleParentFamiliesCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("11. Из неполных семей")));
            var studentsFromSingleParentFamiliesCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(studentsFromSingleParentFamiliesCount));
            for (int i = 0; i < tabCount; i++)
                studentsFromSingleParentFamiliesCountValueRun.Append(new TabChar());
            studentsFromSingleParentFamiliesCountParagraph.Append(studentsFromSingleParentFamiliesCountValueRun);
            _documentBody.Append(studentsFromSingleParentFamiliesCountParagraph);

            var studentsFromRegionsAffectedByChernobylDisasterCount = characteristics.StudentsFromRegionsAffectedByChernobylDisaster == null ? "" :
                ((int)characteristics.StudentsFromRegionsAffectedByChernobylDisaster).ToString();
            if (studentsFromRegionsAffectedByChernobylDisasterCount.Length > 4)
                studentsFromRegionsAffectedByChernobylDisasterCount = studentsFromRegionsAffectedByChernobylDisasterCount.Substring(0, 4);
            tabCount = 1;
            var studentsFromRegionsAffectedByChernobylDisasterCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("12. Из регионов, пострадавших от катастрофы на Чернобыльской АЭС")));
            var studentsFromRegionsAffectedByChernobylDisasterCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new Text("  " + studentsFromRegionsAffectedByChernobylDisasterCount));
            for (int i = 0; i < tabCount; i++)
                studentsFromRegionsAffectedByChernobylDisasterCountValueRun.Append(new TabChar());
            studentsFromRegionsAffectedByChernobylDisasterCountParagraph.Append(studentsFromRegionsAffectedByChernobylDisasterCountValueRun);
            _documentBody.Append(studentsFromRegionsAffectedByChernobylDisasterCountParagraph);

            var studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCount = 
                characteristics.StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution == null ? "" :
                ((int)characteristics.StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution).ToString();
            if (studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCount.Length > 10)
                studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCount = studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCount.Substring(0, 10);
            tabCount = 2 - (Math.Max(0, studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCount.Length - 1) / 5);
            var studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("13. Из семей, отселенных из зон радиоактивного загрязнения")));
            var studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(),
                new Text(studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCount));
            for (int i = 0; i < tabCount; i++)
                studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCountValueRun.Append(new TabChar());
            studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCountParagraph.Append(studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCountValueRun);
            _documentBody.Append(studentsFromFamiliesRelocatedFromAreasOfRadioactivePollutionCountParagraph);

            var nonResidentStudentsCount = characteristics.NonResidentStudents == null ? "" : ((int)characteristics.NonResidentStudents).ToString();
            if (nonResidentStudentsCount.Length > 45) nonResidentStudentsCount = nonResidentStudentsCount.Substring(0, 45);
            tabCount = 9 - (Math.Max(0, nonResidentStudentsCount.Length - 1) / 5);
            var nonResidentStudentsCountParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("14. Иногородних")));
            var nonResidentStudentsCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(nonResidentStudentsCount));
            for (int i = 0; i < tabCount; i++)
                nonResidentStudentsCountValueRun.Append(new TabChar());
            nonResidentStudentsCountParagraph.Append(nonResidentStudentsCountValueRun);
            _documentBody.Append(nonResidentStudentsCountParagraph);

            var studentsLivingWithParentsCount = characteristics.StudentsLivingWithParents == null ? "" : 
                ((int)characteristics.StudentsLivingWithParents).ToString();
            if (studentsLivingWithParentsCount.Length > 10) studentsLivingWithParentsCount = studentsLivingWithParentsCount.Substring(0, 10);
            tabCount = 2 - (Math.Max(0, studentsLivingWithParentsCount.Length - 1) / 5);
            var fifteenthPointParagraph1 = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("15. Проживают с родителями ")));
            var studentsLivingWithParentsCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(studentsLivingWithParentsCount));
            for (int i = 0; i < tabCount; i++)
                studentsLivingWithParentsCountValueRun.Append(new TabChar());
            fifteenthPointParagraph1.Append(studentsLivingWithParentsCountValueRun);

            var studentsLivingInADormitoryCount = characteristics.StudentsLivingInADormitory == null ? "" :
                ((int)characteristics.StudentsLivingInADormitory).ToString();
            if (studentsLivingInADormitoryCount.Length > 10) studentsLivingInADormitoryCount = studentsLivingInADormitoryCount.Substring(0, 10);
            tabCount = 2 - (Math.Max(0, studentsLivingInADormitoryCount.Length - 1) / 5);
            var studentsLivingInADormitoryLabelRun = new Run(WordUtils.GetRunProperties(),
                new Text(", в общежитии "));
            var studentsLivingInADormitoryValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(),
                new Text(studentsLivingInADormitoryCount));
            for (int i = 0; i < tabCount; i++)
                studentsLivingInADormitoryValueRun.Append(new TabChar());
            var commaRun = new Run(WordUtils.GetRunProperties(),
                new Text(","));
            fifteenthPointParagraph1.Append(studentsLivingInADormitoryLabelRun, studentsLivingInADormitoryValueRun, commaRun);

            _documentBody.Append(fifteenthPointParagraph1);

            var studentsLivingWithRelativesCount = characteristics.StudentsLivingWithRelatives == null ? "" :
                ((int)characteristics.StudentsLivingWithRelatives).ToString();
            if (studentsLivingWithRelativesCount.Length > 10) studentsLivingWithRelativesCount = studentsLivingWithRelativesCount.Substring(0, 10);
            tabCount = 2 - (Math.Max(0, studentsLivingWithRelativesCount.Length - 1) / 5);
            var fifteenthPointParagraph2 = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("у родственников ")));
            var studentsLivingWithRelativesCountValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(studentsLivingWithRelativesCount));
            for (int i = 0; i < tabCount; i++)
                studentsLivingWithRelativesCountValueRun.Append(new TabChar());
            fifteenthPointParagraph2.Append(studentsLivingWithRelativesCountValueRun);

            var studentsLivingInPrivateApartmentsCount = characteristics.StudentsLivingInPrivateApartments == null ? "" :
                ((int)characteristics.StudentsLivingInPrivateApartments).ToString();
            if (studentsLivingInPrivateApartmentsCount.Length > 10)
                studentsLivingInPrivateApartmentsCount = studentsLivingInPrivateApartmentsCount.Substring(0, 10);
            tabCount = 2 - (Math.Max(0, studentsLivingInPrivateApartmentsCount.Length - 1) / 5);
            var studentsLivingInPrivateApartmentsLabelRun = new Run(WordUtils.GetRunProperties(),
                new Text(", на частных квартирах "));
            var studentsLivingInPrivateApartmentsValueRun = new Run(WordUtils.GetRunProperties(underline: true),
                new TabChar(), new TabChar(),
                new Text(studentsLivingInPrivateApartmentsCount));
            for (int i = 0; i < tabCount; i++)
                studentsLivingInPrivateApartmentsValueRun.Append(new TabChar());
            var dotRun = new Run(WordUtils.GetRunProperties(),
                new Text("."));
            fifteenthPointParagraph2.Append(studentsLivingInPrivateApartmentsLabelRun, studentsLivingInPrivateApartmentsValueRun, dotRun);

            _documentBody.Append(fifteenthPointParagraph2);

            //int maxCharactersCount = 45 + 5 * 65;
            //var otherInfoStr = characteristics.OtherInformation == null ? "" : characteristics.OtherInformation;
            //if (otherInfoStr.Length > maxCharactersCount) otherInfoStr = otherInfoStr.Substring(0, maxCharactersCount);
            //tabCount = (9 + 5 * 13) - (Math.Max(0, otherInfoStr.Length - 1) / 5);
            //var otherInfoParagraph = new Paragraph(paragraphProperties.CloneNode(true),
            //    new Run(WordUtils.GetRunProperties(),
            //        new Text("16. Другие сведения ")));
            //var otherInfoValueRun = new Run(WordUtils.GetRunProperties(underline: true),
            //    new TabChar(),
            //    new Text(otherInfoStr));
            //for (int i = 0; i < tabCount; i++)
            //    otherInfoValueRun.Append(new TabChar());
            //otherInfoParagraph.Append(otherInfoValueRun);
            //_documentBody.Append(otherInfoParagraph);

            int maxCharactersCount = 45 + 5 * 65;
            var otherInfoStr = characteristics.OtherInformation == null ? "" : characteristics.OtherInformation;
            if (otherInfoStr.Length > maxCharactersCount) otherInfoStr = otherInfoStr.Substring(0, maxCharactersCount);

            Tabs tabs = new Tabs();
            tabs.Append(new TabStop()
            {
                Val = TabStopValues.Right,
                Position = 9225
            });

            var otherInfoParagraphProperties = paragraphProperties.CloneNode(true);
            otherInfoParagraphProperties.Append(tabs);
            var otherInfoParagraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new Text("16. Другие сведения ")));

            var otherInfoRun = new Run(WordUtils.GetRunProperties(underline: true), new TabChar());

            int linesCount = 6;
            if (otherInfoStr != "")
            {
                var split = otherInfoStr.Split(' ');
                var lines = new List<string>();
                string currentLine = "";
                int lineMaxChars = 45;

                foreach (var word in split)
                {
                    if (currentLine.Length + word.Length + 1 <= lineMaxChars)
                    {
                        currentLine += word + " ";
                    }
                    else
                    {
                        lines.Add(currentLine);
                        currentLine = word + " ";
                        lineMaxChars = 65;
                    }
                }

                if (!string.IsNullOrEmpty(currentLine) && !lines.Contains(currentLine))
                {
                    lines.Add(currentLine);
                }

                linesCount = Math.Min(linesCount, lines.Count);
                for (int i = 0; i < linesCount; i++)
                {
                    var lineStr = lines[i];

                    int otherInfoTabCount = i == 0
                        ? 9 - (Math.Max(0, lineStr.Length - 1) / 5)
                        : 13 - (Math.Max(0, lineStr.Length - 1) / 5);

                    if (i == 0)
                    {
                        otherInfoRun.Append(new Text(lineStr));
                        for (int j = 0; j < otherInfoTabCount; j++)
                            otherInfoRun.Append(new TabChar());
                        otherInfoParagraph.Append(otherInfoRun);
                        _documentBody.Append(otherInfoParagraph);
                    }
                    else
                    {
                        var newParagraph = new Paragraph(otherInfoParagraphProperties.CloneNode(true));
                        var newValueRun = new Run(WordUtils.GetRunProperties(underline: true), new Text(lineStr));

                        //for (int j = 0; j < otherInfoTabCount; j++)
                        //{
                        //    newValueRun.Append(new TabChar());
                        //}
                        newValueRun.Append(new TabChar());

                        newParagraph.Append(newValueRun);
                        _documentBody.Append(newParagraph);
                    }
                }

                AppendEmptyLines(6 - linesCount);
            }
            else
            {
                otherInfoRun.Append(new Text(""));
                for (int j = 0; j < 9; j++)
                    otherInfoRun.Append(new TabChar());
                otherInfoParagraph.Append(otherInfoRun);
                _documentBody.Append(otherInfoParagraph);
                AppendEmptyLines(linesCount - 1);
            }
        }

        private void AppendEmptyLines(int count)
        {
            Tabs tabs = new Tabs();
            tabs.Append(new TabStop()
            {
                Val = TabStopValues.Right,
                Position = 9225
            });
            for (int i = 0; i < count; i++)
            {
                var emptyParagraph = new Paragraph(new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both },
                    new SpacingBetweenLines() { Before = "180", After = "180" }, tabs.CloneNode(true)
                ));
                var emptyRun = new Run(WordUtils.GetRunProperties(underline: true));
                //for (int j = 0; j < 13; j++)
                //{
                //    emptyRun.Append(new TabChar());
                //}
                emptyRun.Append(new TabChar());
                emptyParagraph.Append(emptyRun);
                _documentBody.Append(emptyParagraph);
            }
        }
    }
}
