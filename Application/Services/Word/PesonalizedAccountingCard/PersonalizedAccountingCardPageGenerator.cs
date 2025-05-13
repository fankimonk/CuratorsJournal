using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent.Pages;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Domain.Enums.Journal;

namespace Application.Services.Word.PesonalizedAccountingCard
{
    public class PersonalizedAccountingCardPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private readonly int _maxAddressLines = 4;
        private int _totalAddressLines = 0;

        private readonly int _maxGraduatedLines = 2;
        private int _totalGraduatedLines = 0;

        public PersonalizedAccountingCardPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.PersonalizedAccountingCard);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                GeneratePage(page);
            }
            else
            {
                foreach (var p in pages)
                {
                    GeneratePage(p);

                    if (p != pages.Last()) WordUtils.AppendPageBreak(_documentBody);
                }
            } 
        }

        private void GeneratePage(Page page)
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

            //WordUtils.AppendPageBreak(_documentBody);

            var individualWorkWithStudentGenerator = new IndividualWorkWithStudentGenerator(page.PersonalizedAccountingCard.IndividualWorkWithStudent, _documentBody);
            individualWorkWithStudentGenerator.Generate();

            WordUtils.AppendBreaks(1, _documentBody);

            var workWithParentsGenerator = new WorkWithParentsGenerator(page.PersonalizedAccountingCard.WorkWithParents, _documentBody);
            workWithParentsGenerator.Generate();
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines { After = "0"} ),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("КАРТА ПЕРСОНИФИЦИРОВАННОГО УЧЕТА"))
            );

            _documentBody.Append(title);
        }

        private void AppendContent(PersonalizedAccountingCard card)
        {
            var content = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both },
                    new SpacingBetweenLines { Before = "0", After = "0" })
            );

            var fioLabelRun = new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Фамилия, имя отчество"));
            var fioStr = card.Student == null ? "" : GetStudentFIO(card.Student);
            var fioRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
                    new TabChar(), new TabChar(),
                    new Text(fioStr));
            int tabCount = 21 - Math.Max(0, fioStr.Length - 1) / 5;
            for (int i = 0; i < tabCount; i++) fioRun.Append(new TabChar());
            fioRun.Append(new Break());

            var birthDateLabelRun = new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text("Дата рождения"));
            var birthDateStr = card.BirthDate == null ? "" : ((DateOnly)card.BirthDate).ToString();
            var birthDateRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
                    new TabChar(),
                    new Text(birthDateStr));
            tabCount = card.BirthDate == null ? 10 : 9;
            for (int i = 0; i < tabCount; i++) birthDateRun.Append(new TabChar());
            birthDateRun.Append(new Break());

            var passportDataLabelRun = new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text("Паспортные данные"));
            var passportDataStr = card.PassportData ?? "";
            var passportDataRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
                    new TabChar(),
                    new Text(passportDataStr));
            tabCount = 15 - Math.Max(0, passportDataStr.Length - 1) / 5;
            for (int i = 0; i < tabCount; i++) passportDataRun.Append(new TabChar());

            var citizenshipLabelRun = new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text("гражданство"));
            var citizenshipStr = card.Citizenship ?? "";
            var citizenshipRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
                    new TabChar(), new TabChar(),
                    new Text(citizenshipStr));
            tabCount = 4 - Math.Max(0, citizenshipStr.Length - 1) / 5;
            for (int i = 0; i < tabCount; i++) citizenshipRun.Append(new TabChar());
            citizenshipRun.Append();

            //var graduatedLabelRun = new Run(WordUtils.GetRunProperties(fontSize: "26"),
            //        new Text("Окончил УО"));
            //var graduatedStr = card.GraduatedEducationalInstitution ?? "";
            //var graduatedRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
            //        new TabChar(),
            //        new Text(card.GraduatedEducationalInstitution == null ? "" : card.GraduatedEducationalInstitution));
            //tabCount = 23 - Math.Max(0, graduatedStr.Length - 1) / 5;
            //for (int i = 0; i < tabCount; i++) graduatedRun.Append(new TabChar());
            //graduatedRun.Append(new Break());

            //var addressLabelRun = new Run(WordUtils.GetRunProperties(fontSize: "26"),
            //        new Text("Место и адрес проживания в период обучения"));
            //var addressStr = card.ResidentialAddress ?? "";
            //var addressRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
            //        new TabChar(),
            //        new Text(addressStr));
            //tabCount = (5 + 13 * 3) - Math.Max(0, addressStr.Length - 1) / 5;
            //for (int i = 0; i < tabCount; i++) addressRun.Append(new TabChar());

            content.Append(fioLabelRun, fioRun);
            content.Append(birthDateLabelRun, birthDateRun);
            content.Append(passportDataLabelRun, passportDataRun);
            content.Append(citizenshipLabelRun, citizenshipRun);
            //content.Append(graduatedLabelRun, graduatedRun);
            //content.Append(addressLabelRun, addressRun);

            _documentBody.Append(content);
            AppendGraduated(card.GraduatedEducationalInstitution ?? "");
            AppendAddress(card.ResidentialAddress ?? "");

            var healthStatus = CreateSectionTitle("Сведения о состоянии здоровья");

            _documentBody.Append(healthStatus);
            AppendChronicDisesaes(card.Student == null ? [] : card.Student.ChronicDiseases, healthStatus);
            AppendPEGroups(card.Student == null ? [] : card.Student.PEGroups, healthStatus);

        }

        private Paragraph CreateSectionTitle(string text)
        {
            return new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Start },
                    new SpacingBetweenLines() { Before = "25", After = "25" }),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text(text))
            );
        }

        private void AppendChronicDisesaes(List<ChronicDisease> chronicDiseases, Paragraph healthStatusParagraph)
        {
            //var chronicDiseasesLabel = new Run(WordUtils.GetRunProperties(fontSize: "26"),
            //    new Text("Хронические заболевания"));
            var text = "";
            foreach (var cd in chronicDiseases)
            {
                text += cd == chronicDiseases.Last() ? cd.Name : cd.Name + ", ";
            }
            //var chronicDiseasesRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
            //    new TabChar(),
            //    new Text(text));
            //int tabCount = 21 - Math.Max(0, text.Length - 1) / 5;
            //for (int i = 0; i < tabCount; i++) chronicDiseasesRun.Append(new TabChar());
            //chronicDiseasesRun.Append(new Break());
            //healthStatusParagraph.Append(chronicDiseasesLabel, chronicDiseasesRun);
            AppendChronicDiseases(text);
        }

        private void AppendPEGroups(List<PEGroup> peGroups, Paragraph healthStatusParagraph)
        {
            //var peGroupsLabel = new Run(WordUtils.GetRunProperties(fontSize: "26"),
            //    new Text("Группы по физической культуре (основная специальная)"));
            var text = "";
            foreach (var group in peGroups)
            {
                text += group == peGroups.Last() ? group.Name : group.Name + ", ";
            }
            //var peGroupsRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
            //    new TabChar(), new TabChar(),
            //    new Text(text));
            //int tabCount = 16 - Math.Max(0, text.Length - 1) / 5;
            //for (int i = 0; i < tabCount; i++) peGroupsRun.Append(new TabChar());
            //healthStatusParagraph.Append(peGroupsLabel, peGroupsRun);
            AppendPEGroups(text);
        }

        private string GetStudentFIO(Student student)
        {
            return student.LastName + " " + student.FirstName + " " + student.MiddleName;
        }

        private void AppendChronicDiseases(string diseasesStr)
        {
            int maxCharactersCount = 45 + 65;
            if (diseasesStr.Length > maxCharactersCount) diseasesStr = diseasesStr.Substring(0, maxCharactersCount);

            var spacing = new SpacingBetweenLines() { Before = "0", After = "0" };
            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }, spacing);

            var paragraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text("Хронические заболевания")));

            var run = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
                    new TabChar());

            int totalLines = 0;
            int maxLines = 2;
            if (diseasesStr != "")
            {
                var split = diseasesStr.Split(' ');
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

                if (totalLines + lines.Count > maxLines) return;
                totalLines += lines.Count;

                for (int i = 0; i < lines.Count; i++)
                {
                    var lineStr = lines[i];

                    int tabCount = i == 0
                        ? 8 - (Math.Max(0, lineStr.Length - 1) / 5)
                        : 13 - (Math.Max(0, lineStr.Length - 1) / 5);

                    if (i == 0)
                    {
                        run.Append(new Text(lineStr));
                        for (int j = 0; j < tabCount; j++)
                            run.Append(new TabChar());
                        paragraph.Append(run);
                        _documentBody.Append(paragraph);
                    }
                    else
                    {
                        var newParagraph = new Paragraph(paragraphProperties.CloneNode(true));
                        var newValueRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"), new Text(lineStr));

                        for (int j = 0; j < tabCount; j++)
                        {
                            newValueRun.Append(new TabChar());
                        }

                        newParagraph.Append(newValueRun);
                        _documentBody.Append(newParagraph);
                    }
                }

                AppendEmptyLines(maxLines - lines.Count);
            }
            else
            {
                run.Append(new Text(""));
                for (int j = 0; j < 8; j++)
                    run.Append(new TabChar());
                paragraph.Append(run);
                _documentBody.Append(paragraph);
                AppendEmptyLines(maxLines - 1);
            }
        }

        private void AppendPEGroups(string peGroupsStr)
        {
            int maxCharactersCount = 16 + 65;
            if (peGroupsStr.Length > maxCharactersCount) peGroupsStr = peGroupsStr.Substring(0, maxCharactersCount);

            var spacing = new SpacingBetweenLines() { Before = "0", After = "0" };
            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }, spacing);

            var paragraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text("Группы по физической культуре (основная специальная)")));

            var run = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
                    new TabChar(), new TabChar());

            int totalLines = 0;
            int maxLines = 2;
            if (peGroupsStr != "")
            {
                var split = peGroupsStr.Split(' ');
                var lines = new List<string>();
                string currentLine = "";
                int lineMaxChars = 16;

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

                if (totalLines + lines.Count > maxLines) return;
                totalLines += lines.Count;

                for (int i = 0; i < lines.Count; i++)
                {
                    var lineStr = lines[i];

                    int tabCount = i == 0
                        ? 3 - (Math.Max(0, lineStr.Length - 1) / 5)
                        : 13 - (Math.Max(0, lineStr.Length - 1) / 5);

                    if (i == 0)
                    {
                        run.Append(new Text(lineStr));
                        for (int j = 0; j < tabCount; j++)
                            run.Append(new TabChar());
                        paragraph.Append(run);
                        _documentBody.Append(paragraph);
                    }
                    else
                    {
                        var newParagraph = new Paragraph(paragraphProperties.CloneNode(true));
                        var newValueRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"), new Text(lineStr));

                        for (int j = 0; j < tabCount; j++)
                        {
                            newValueRun.Append(new TabChar());
                        }

                        newParagraph.Append(newValueRun);
                        _documentBody.Append(newParagraph);
                    }
                }

                AppendEmptyLines(maxLines - lines.Count);
            }
            else
            {
                run.Append(new Text(""));
                for (int j = 0; j < 3; j++)
                    run.Append(new TabChar());
                paragraph.Append(run);
                _documentBody.Append(paragraph);
                AppendEmptyLines(maxLines - 1);
            }
        }

        private void AppendGraduated(string graduatedStr)
        {
            _totalGraduatedLines = 0;

            int maxCharactersCount = 52 + 65;
            if (graduatedStr.Length > maxCharactersCount) graduatedStr = graduatedStr.Substring(0, maxCharactersCount);

            var spacing = new SpacingBetweenLines() { Before = "0", After = "0" };
            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }, spacing);

            var paragraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text("Окончил УО")));

            var addressRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
                    new TabChar());

            if (graduatedStr != "")
            {
                var split = graduatedStr.Split(' ');
                var lines = new List<string>();
                string currentLine = "";
                int lineMaxChars = 52;

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

                if (_totalGraduatedLines + lines.Count > _maxGraduatedLines) return;
                _totalGraduatedLines += lines.Count;

                for (int i = 0; i < lines.Count; i++)
                {
                    var lineStr = lines[i];

                    int tabCount = i == 0
                        ? 10 - (Math.Max(0, lineStr.Length - 1) / 5)
                        : 13 - (Math.Max(0, lineStr.Length - 1) / 5);

                    if (i == 0)
                    {
                        addressRun.Append(new Text(lineStr));
                        for (int j = 0; j < tabCount; j++)
                            addressRun.Append(new TabChar());
                        paragraph.Append(addressRun);
                        _documentBody.Append(paragraph);
                    }
                    else
                    {
                        var newParagraph = new Paragraph(paragraphProperties.CloneNode(true));
                        var newValueRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"), new Text(lineStr));

                        for (int j = 0; j < tabCount; j++)
                        {
                            newValueRun.Append(new TabChar());
                        }

                        newParagraph.Append(newValueRun);
                        _documentBody.Append(newParagraph);
                    }
                }

                AppendEmptyLines(_maxGraduatedLines - lines.Count);
            }
            else
            {
                addressRun.Append(new Text(""));
                for (int j = 0; j < 10; j++)
                    addressRun.Append(new TabChar());
                paragraph.Append(addressRun);
                _documentBody.Append(paragraph);
                AppendEmptyLines(_maxGraduatedLines - 1);
            }
        }

        private void AppendAddress(string addressStr)
        {
            _totalAddressLines = 0;

            int maxCharactersCount = 26 + 3 * 65;
            if (addressStr.Length > maxCharactersCount) addressStr = addressStr.Substring(0, maxCharactersCount);

            var spacing = new SpacingBetweenLines() { Before = "0", After = "0" };
            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }, spacing);

            var paragraph = new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text("Место и адрес проживания в период обучения")));

            var addressRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"),
                    new TabChar());

            if (addressStr != "")
            {
                var split = addressStr.Split(' ');
                var lines = new List<string>();
                string currentLine = "";
                int lineMaxChars = 26;

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

                if (_totalAddressLines + lines.Count > _maxAddressLines) return;
                _totalAddressLines += lines.Count;

                for (int i = 0; i < lines.Count; i++)
                {
                    var lineStr = lines[i];

                    int tabCount = i == 0
                        ? 5 - (Math.Max(0, lineStr.Length - 1) / 5)
                        : 13 - (Math.Max(0, lineStr.Length - 1) / 5);

                    if (i == 0)
                    {
                        addressRun.Append(new Text(lineStr));
                        for (int j = 0; j < tabCount; j++)
                            addressRun.Append(new TabChar());
                        paragraph.Append(addressRun);
                        _documentBody.Append(paragraph);
                    }
                    else
                    {
                        var newParagraph = new Paragraph(paragraphProperties.CloneNode(true));
                        var newValueRun = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "26"), new Text(lineStr));

                        for (int j = 0; j < tabCount; j++)
                        {
                            newValueRun.Append(new TabChar());
                        }

                        newParagraph.Append(newValueRun);
                        _documentBody.Append(newParagraph);
                    }  
                }

                AppendEmptyLines(_maxAddressLines - lines.Count);
            }
            else
            {
                addressRun.Append(new Text(""));
                for (int j = 0; j < 5; j++)
                    addressRun.Append(new TabChar());
                paragraph.Append(addressRun);
                _documentBody.Append(paragraph);
                AppendEmptyLines(_maxAddressLines - 1);
            }
        }

        private void AppendEmptyLines(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var emptyParagraph = new Paragraph(new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both },
                    new SpacingBetweenLines() { Before = "0", After = "0" }
                ));
                var emptyRun = new Run(WordUtils.GetRunProperties(underline: true));
                for (int j = 0; j < 13; j++)
                {
                    emptyRun.Append(new TabChar());
                }
                emptyParagraph.Append(emptyRun);
                _documentBody.Append(emptyParagraph);
            }
        }
    }
}
