using Application.Interfaces;
using Application.Utils;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;

namespace Application.Services.Word
{
    public class TitlePageGenerator
    {
        private readonly IJournalsService _journalsService;

        private readonly Body _documentBody;

        private int _journalId;
        private Journal? _journal;

        public TitlePageGenerator(int journalId, Body body, IJournalsService journalsService)
        {
            _journalId = journalId;
            _documentBody = body;
            _journalsService = journalsService;
        }

        public async Task Generate()
        {
            _journal = await InitializeJournalData(_journalId);
            if (_journal == null) throw new ArgumentException(nameof(_journalId));

            WordUtils.AppendBreaks(4, _documentBody);
            AppendEducationalInstitutionName();
            WordUtils.AppendBreaks(2, _documentBody);
            AppendTitleName();
            WordUtils.AppendBreaks(1, _documentBody);
            AppendGroupNumber();
            AppendAdmissionYear();
            AppendCurator();
            AppendDepartment();
            AppendFaculty();
            WordUtils.AppendPageBreak(_documentBody);
        }

        private async Task<Journal?> InitializeJournalData(int journalId)
        {
            var titlePageData = await _journalsService.GetJournalsTitlePageData(journalId);
            if (titlePageData == null) return null;

            return titlePageData.Item2;
        }

        private void AppendEducationalInstitutionName()
        {
            var underlineLineParagraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { After = "0" }
            );

            var underlineLineRunProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new Underline() { Val = UnderlineValues.Single },
                new FontSize() { Val = "28" }
            );

            var underlineLineRun = new Run(
                underlineLineRunProperties,
                new TabChar(), new TabChar(), new TabChar(), new TabChar(), new TabChar(), new TabChar(),
                new Text("БНТУ"),
                new TabChar(), new TabChar(), new TabChar(), new TabChar(), new TabChar(), new TabChar()
            );

            var underlineLine = new Paragraph(
                underlineLineParagraphProperties,
                underlineLineRun
            );

            var labelParagraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0" }
            );

            var labelRunProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new FontSize() { Val = "22" }
            );

            var labelParagraph = new Paragraph(
                labelParagraphProperties,
                new Run(
                    labelRunProperties,
                    new Text("наименование учреждения образования"))
            );

            _documentBody.Append(underlineLine);
            _documentBody.Append(labelParagraph);
        }

        private void AppendTitleName()
        {
            var runProperties1 = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new FontSize() { Val = "48" },
                new Bold()
            );

            var runProperties2 = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new FontSize() { Val = "48" },
                new Bold()
            );

            var titleName = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(
                    runProperties1,
                    new Text("ЖУРНАЛ"),
                    new Break()),
                new Run(
                    runProperties2,
                    new Text("КУРАТОРА УЧЕБНОЙ ГРУППЫ"))
            );

            _documentBody.Append(titleName);
        }

        private void AppendGroupNumber()
        {
            if (_journal == null || _journal.Group == null) throw new ArgumentException(nameof(_journal));

            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }
            );

            var labelRunProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new FontSize() { Val = "28" }
            );

            var labelRun = new Run(labelRunProperties,
                new Text("ГРУППА №"));

            var valueRunProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new Underline() { Val = UnderlineValues.Single },
                new FontSize() { Val = "28" }
            );

            var valueRun = new Run(valueRunProperties,
                new TabChar(), 
                new Text(_journal.Group.Number.ToString()),
                new TabChar(), new TabChar(), new TabChar(), new TabChar(), new TabChar(),
                new TabChar(), new TabChar(), new TabChar(), new TabChar());

            var paragraph = new Paragraph(paragraphProperties,
                labelRun, valueRun);

            _documentBody.Append(paragraph);
        }

        private void AppendAdmissionYear()
        {
            if (_journal == null || _journal.Group == null) throw new ArgumentException(nameof(_journal));

            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }
            );

            var labelRunProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new FontSize() { Val = "28" }
            );

            var labelRun = new Run(labelRunProperties,
                new Text("ГОД ПОСТУПЛЕНИЯ"));

            var valueRunProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new Underline() { Val = UnderlineValues.Single },
                new FontSize() { Val = "28" }
            );

            var valueRun = new Run(valueRunProperties,
                new TabChar(), new TabChar(),
                new Text(_journal.Group.AdmissionYear.ToString()),
                new TabChar(), new TabChar(), new TabChar(), new TabChar(),
                new TabChar(), new TabChar(), new TabChar(), new TabChar());

            var paragraph = new Paragraph(paragraphProperties,
                labelRun, valueRun);

            _documentBody.Append(paragraph);
        }

        private void AppendCurator()
        {
            if (_journal == null || _journal.Group == null) throw new ArgumentException(nameof(_journal));

            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }
            );

            var labelRun = new Run(WordUtils.GetRunProperties(),
                new Text("КУРАТОР"));

            var paragraph = new Paragraph(paragraphProperties, labelRun);

            var valueRunProperties = WordUtils.GetRunProperties(underline: true);

            string curatorFIO = "";
            if (_journal.Group.Curator != null && _journal.Group.Curator.Worker != null)
            {
                var worker = _journal.Group.Curator.Worker;
                curatorFIO = worker.LastName + " " + worker.FirstName + " " + worker.MiddleName;
            }

            var valueRun = new Run(valueRunProperties, new TabChar());

            int linesCount = 3;
            if (curatorFIO != "")
            {
                var curatorWords = curatorFIO.Split(' ');
                var lines = new List<string>();
                string currentLine = "";
                int lineMaxChars = 55;

                foreach (var word in curatorWords)
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
                   
                    int tabCount = i == 0
                        ? 11 - (Math.Max(0, lineStr.Length - 1) / 5)
                        : 13 - (Math.Max(0, lineStr.Length - 1) / 5);

                    if (i == 0)
                    {
                        valueRun.Append(new Text(lineStr));
                        for (int j = 0; j < tabCount; j++)
                            valueRun.Append(new TabChar());
                        paragraph.Append(valueRun);
                        _documentBody.Append(paragraph);
                    }
                    else
                    {
                        var newParagraph = new Paragraph(paragraphProperties.CloneNode(true));
                        var newValueRun = new Run(valueRunProperties.CloneNode(true), new Text(lineStr));

                        for (int j = 0; j < tabCount; j++)
                        {
                            newValueRun.Append(new TabChar());
                        }

                        newParagraph.Append(newValueRun);
                        _documentBody.Append(newParagraph);
                    }  
                }

                AppendEmptyLines(3 - linesCount);
            }
            else
            {
                valueRun.Append(new Text(""));
                for (int j = 0; j < 11; j++)
                    valueRun.Append(new TabChar());
                paragraph.Append(valueRun);
                _documentBody.Append(paragraph);
                AppendEmptyLines(linesCount - 1);
            }
        }

        private void AppendEmptyLines(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var emptyParagraph = new Paragraph(new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }
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

        private void AppendDepartment()
        {
            if (_journal == null || _journal.Group == null || _journal.Group.Specialty == null
                || _journal.Group.Specialty.Department == null) throw new ArgumentException(nameof(_journal));

            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }
            );

            var labelRunProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new FontSize() { Val = "28" }
            );

            var labelRun = new Run(labelRunProperties,
                new Text("КАФЕДРА"));

            var valueRunProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new Underline() { Val = UnderlineValues.Single },
                new FontSize() { Val = "28" }
            );

            var department = _journal.Group.Specialty.Department.AbbreviatedName;
            if (department.Length > 50) department = department.Substring(0, 50);

            var valueRun = new Run(valueRunProperties,
                new TabChar(), new TabChar(),
                new Text(department));

            int tabCount = 10 - (Math.Max(0, department.Length - 1) / 5);
            for (int i = 0; i < tabCount; i++)
                valueRun.Append(new TabChar());

            var paragraph = new Paragraph(paragraphProperties,
                labelRun, valueRun);

            _documentBody.Append(paragraph);
        }

        private void AppendFaculty()
        {
            if (_journal == null || _journal.Group == null || _journal.Group.Specialty == null
                || _journal.Group.Specialty.Department == null || _journal.Group.Specialty.Department.Deanery == null
                || _journal.Group.Specialty.Department.Deanery.Faculty == null) throw new ArgumentException(nameof(_journal));

            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }
            );

            var labelRunProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new FontSize() { Val = "28" }
            );

            var labelRun = new Run(labelRunProperties,
                new Text("ФАКУЛЬТЕТ"));

            var valueRunProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new Underline() { Val = UnderlineValues.Single },
                new FontSize() { Val = "28" }
            );

            string faculty = _journal.Group.Specialty.Department.Deanery.Faculty.Name;
            var facultyWords = faculty.Split(' ');
            if (facultyWords.First().ToLower() == "факультет") facultyWords = facultyWords.Skip(1).ToArray();
            faculty = string.Join(' ', facultyWords);
            if (faculty.Length > 50) faculty = faculty.Substring(0, 50);

            var valueRun = new Run(valueRunProperties,
                new TabChar(),
                new Text(faculty));

            int tabCount = 10 - (Math.Max(0, faculty.Length - 1) / 5);
            for (int i = 0; i < tabCount; i++)
                valueRun.Append(new TabChar());

            var paragraph = new Paragraph(paragraphProperties,
                labelRun, valueRun);

            _documentBody.Append(paragraph);
        }
    }
}
