using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Domain.Enums.Journal;

namespace Application.Services.Word.PesonalizedAccountingCard
{
    public class PersonalizedAccountingCardPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public PersonalizedAccountingCardPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.PersonalizedAccountingCard);
            if (pages == null) throw new ArgumentException(nameof(pages));

            foreach (var page in pages)
            {
                if (page.PersonalizedAccountingCard == null) throw new ArgumentException(nameof(page));

                AppendTitle();
                AppendContent(page.PersonalizedAccountingCard);

                var parentalInformationGenerator = new ParentalInfromationGenerator(page.PersonalizedAccountingCard.ParentalInformation, _documentBody);
                parentalInformationGenerator.Generate();

                var individualInformationGenerator = new IndividualInfromationGenerator(page.PersonalizedAccountingCard.IndividualInformation, _documentBody);
                individualInformationGenerator.Generate();

                var studentEncouragementsGenerator = new StudentEncouragementsGenerator(page.PersonalizedAccountingCard.StudentEcouragements, _documentBody);
                studentEncouragementsGenerator.Generate();

                var studentDisciplinaryResonsibilitiesGenerator = new StudentDisciplinaryResonsibilitiesGenerator
                    (page.PersonalizedAccountingCard.StudentDisciplinaryResponsibilities, _documentBody);
                studentDisciplinaryResonsibilitiesGenerator.Generate();

                WordUtils.AppendPageBreak(_documentBody);

                var individualWorkWithStudentGenerator = new IndividualWorkWithStudentGenerator(page.PersonalizedAccountingCard.IndividualWorkWithStudent, _documentBody);
                individualWorkWithStudentGenerator.Generate();

                WordUtils.AppendBreaks(1, _documentBody);

                var workWithParentsGenerator = new WorkWithParentsGenerator(page.PersonalizedAccountingCard.WorkWithParents, _documentBody);
                workWithParentsGenerator.Generate();

                if (page != pages.Last()) WordUtils.AppendPageBreak(_documentBody);
            }
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("КАРТА ПЕРСОНИФИЦИРОВАННОГО УЧЕТА"))
            );

            _documentBody.Append(title);
        }

        private void AppendContent(PersonalizedAccountingCard card)
        {
            var content = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Фамилия, имя отчество")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(card.Student == null ? "" : GetStudentFIO(card.Student)),
                    new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("Дата рождения")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(card.BirthDate == null ? "" : ((DateOnly)card.BirthDate).ToString()),
                    new TabChar(), new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("Паспортные данные")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(card.PassportData == null ? "" : card.PassportData),
                    new TabChar(), new TabChar()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("гражданство")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(card.Citizenship == null ? "" : card.Citizenship),
                    new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("Окончил УО")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(), new TabChar(),
                    new Text(card.GraduatedEducationalInstitution == null ? "" : card.GraduatedEducationalInstitution),
                    new TabChar(), new TabChar(),
                    new Break()),
                new Run(WordUtils.GetRunProperties(),
                    new Text("Место и адрес проживания в период обучения")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(card.ResidentialAddress == null ? "" : card.ResidentialAddress),
                    new TabChar())
            );

            var healthStatus = CreateSectionTitle("Сведения о состоянии здоровья");

            AppendChronicDisesaes(card.Student == null ? [] : card.Student.ChronicDiseases, healthStatus);
            AppendPEGroups(card.Student == null ? [] : card.Student.PEGroups, healthStatus);

            _documentBody.Append(content, healthStatus);
        }

        private Paragraph CreateSectionTitle(string text)
        {
            return new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Start }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text(text),
                    new Break())
            );
        }

        private void AppendChronicDisesaes(List<ChronicDisease> chronicDiseases, Paragraph healthStatusParagraph)
        {
            var chronicDiseasesLabel = new Run(WordUtils.GetRunProperties(),
                new Text("Хронические заболевания"));
            var tabRun1 = new Run(WordUtils.GetRunProperties(underline: true), new TabChar());
            healthStatusParagraph.Append(chronicDiseasesLabel, tabRun1);

            foreach (var cd in chronicDiseases)
            {
                var text = cd == chronicDiseases.Last() ? cd.Name : cd.Name + ", ";
                var cdRun = new Run(WordUtils.GetRunProperties(underline: true),
                    new Text(text));
                healthStatusParagraph.Append(cdRun);
            }

            var tabRun2 = new Run(WordUtils.GetRunProperties(underline: true), new TabChar(), new Break());
            healthStatusParagraph.Append(tabRun2);
        }

        private void AppendPEGroups(List<PEGroup> peGroups, Paragraph healthStatusParagraph)
        {
            var peGroupLabel = new Run(WordUtils.GetRunProperties(),
                new Text("Группы по физической культуре (основная специальная)"));
            var tabRun1 = new Run(WordUtils.GetRunProperties(underline: true), new TabChar());
            healthStatusParagraph.Append(peGroupLabel, tabRun1);

            foreach (var group in peGroups)
            {
                var text = group == peGroups.Last() ? group.Name : group.Name + ", ";
                var groupRun = new Run(WordUtils.GetRunProperties(underline: true),
                    new Text(text));
                healthStatusParagraph.Append(groupRun);
            }

            var tabRun2 = new Run(WordUtils.GetRunProperties(underline: true), new TabChar());
            healthStatusParagraph.Append(tabRun2);
        }

        private string GetStudentFIO(Student student)
        {
            return student.LastName + " " + student.FirstName + " " + student.MiddleName;
        }
    }
}
