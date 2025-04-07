using Application.Interfaces;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
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

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.SocioPedagogicalCharacteristics);
            if (pages == null) throw new ArgumentException(nameof(pages));

            var group = await _groupsRepository.GetByJournalId(_journalId);
            if (group == null) throw new ArgumentException(nameof(group));

            foreach (var page in pages)
            {
                if (page.SocioPedagogicalCharacteristicsPageAttributes == null) throw new ArgumentException(nameof(page));
                if (page.SocioPedagogicalCharacteristics == null) throw new ArgumentException(nameof(page));

                AppendTitle(group.Number, page.SocioPedagogicalCharacteristicsPageAttributes.AcademicYear);
                WordUtils.AppendBreaks(1, _documentBody);

                AppendContent(page.SocioPedagogicalCharacteristics);

                WordUtils.AppendPageBreak(_documentBody);
            }
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

            if (academicYear != null)
            {
                var startYear = academicYear.StartYear.ToString();
                var endYear = academicYear.EndYear.ToString();

                title.Append(new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("в " + startYear.Substring(0, startYear.Length - 2))));
                title.Append(new Run(WordUtils.GetRunProperties(underline: true, bold: true),
                    new Text(startYear.Substring(startYear.Length - 2))));
                title.Append(new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("/ " + endYear.Substring(0, endYear.Length - 2))));
                title.Append(new Run(WordUtils.GetRunProperties(underline: true, bold: true),
                    new Text(endYear.Substring(endYear.Length - 2))));
            }
            else
            {
                title.Append(new Run(WordUtils.GetRunProperties(underline: true, bold: true),
                    new TabChar(), new TabChar()));
            }

            title.Append(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("уч. году")));

            _documentBody.Append(title);
        }

        private void AppendContent(SocioPedagogicalCharacteristics characteristics)
        {
            var content = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both }),
                new Run(WordUtils.GetRunProperties(),
                    new Text("1. Всего студентов")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(characteristics.TotalStudents == null ? "" : ((int)characteristics.TotalStudents).ToString()),
                    new TabChar(), new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("Из них"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("2. Девушки")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(characteristics.FemalesCount == null ? "" : ((int)characteristics.FemalesCount).ToString()),
                    new TabChar(), new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("3. Юноши")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(characteristics.MalesCount == null ? "" : ((int)characteristics.MalesCount).ToString()),
                    new TabChar(), new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("4. Члены ОО \"БРСМ\"")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(characteristics.BRSMMembersCount == null ? "" : ((int)characteristics.BRSMMembersCount).ToString()),
                    new TabChar(), new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("5. Дети-сироты, (до 18 лет)")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(characteristics.OrphansUnderagesCount == null ? "" : ((int)characteristics.OrphansUnderagesCount).ToString()),
                    new TabChar(), new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("6. Дети, оставшиеся без попечения родителей (до 18 лет)")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.StudentsWithoutParentalCareUnderagesCount == null ? "" : ((int)characteristics.StudentsWithoutParentalCareUnderagesCount).ToString()),
                    new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("7. Лица из числа детей-сирот"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("и детей, оставшихся без попечения родителей (18-23 года)")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.OrphansOrStudentsWithoutParentalCareAdults == null ? "" : ((int)characteristics.OrphansOrStudentsWithoutParentalCareAdults).ToString()),
                    new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("8. Студенты с особенностями психофизического развития")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.StudentsWithSpecialPsychophysicalDevelopmentNeeds == null ? "" : ((int)characteristics.StudentsWithSpecialPsychophysicalDevelopmentNeeds).ToString()),
                    new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("9. Имеющие родителей-инвалидов 1, 2 группы")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.StudentsWithDisabledParentsOfGroups1and2 == null ? "" : ((int)characteristics.StudentsWithDisabledParentsOfGroups1and2).ToString()),
                    new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("10. Из многодетных семей")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(characteristics.StudentsFromLargeFamilies == null ? "" : ((int)characteristics.StudentsFromLargeFamilies).ToString()),
                    new TabChar(), new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("11. Из неполных семей")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(characteristics.StudentsFromSingleParentFamilies == null ? "" : ((int)characteristics.StudentsFromSingleParentFamilies).ToString()),
                    new TabChar(), new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("12. Из регионов, пострадавших от катастрофы на Чернобыльской АЭС")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.StudentsFromRegionsAffectedByChernobylDisaster == null ? "" : ((int)characteristics.StudentsFromRegionsAffectedByChernobylDisaster).ToString()),
                    new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("13. Из семей, отселенных из зон радиоактивного загрязнения")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution == null ? "" : ((int)characteristics.StudentsFromFamiliesRelocatedFromAreasOfRadioactivePollution).ToString()),
                    new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("14. Иногородних")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(characteristics.NonResidentStudents == null ? "" : ((int)characteristics.NonResidentStudents).ToString()),
                    new TabChar(), new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("15. Проживают с родителями ")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.StudentsLivingWithParents == null ? "" : ((int)characteristics.StudentsLivingWithParents).ToString()),
                    new TabChar()),
                new Run(WordUtils.GetRunProperties(),
                    new Text(", в общежитии ")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.StudentsLivingInADormitory == null ? "" : ((int)characteristics.StudentsLivingInADormitory).ToString()),
                    new TabChar()),
                new Run(WordUtils.GetRunProperties(),
                    new Text(","),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("у родственников ")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.StudentsLivingWithRelatives == null ? "" : ((int)characteristics.StudentsLivingWithRelatives).ToString()),
                    new TabChar()),
                new Run(WordUtils.GetRunProperties(),
                    new Text(", на частных квартирах ")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.StudentsLivingInPrivateApartments == null ? "" : ((int)characteristics.StudentsLivingInPrivateApartments).ToString()),
                    new TabChar()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("."),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("16. Другие сведения ")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new Text(characteristics.OtherInformation == null ? "" : characteristics.OtherInformation),
                    new TabChar())
            );

            _documentBody.Append(content);
        }
    }
}
