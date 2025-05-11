using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class StudentHealthCardPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public StudentHealthCardPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.StudentsHealthCard);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                if (page.HealthCardPageAttributes == null) throw new ArgumentException(nameof(page));

                AppendTitle(page.HealthCardPageAttributes.AcademicYear);

                AppendTable(page.StudentsHealthCards);

                WordUtils.AppendPageBreak(_documentBody);
            }
        }

        private void AppendTitle(AcademicYear? academicYear)
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("КАРТА ЗДОРОВЬЯ СТУДЕНТОВ В "))
            );

            var startYearPrefixRun = new Run(WordUtils.GetRunProperties(bold: true),
                new Text("20"));
            var startYearRun = new Run(WordUtils.GetRunProperties(underline: true, bold: true));

            var endYearPrefixRun = new Run(WordUtils.GetRunProperties(bold: true),
                new Text("/ 20"));
            var endYearRun = new Run(WordUtils.GetRunProperties(underline: true, bold: true));

            if (academicYear != null)
            {
                var startYear = academicYear.StartYear.ToString();
                startYearRun.Append(new Text(startYear.Substring(startYear.Length - 2, 2)));

                var endYear = academicYear.EndYear.ToString();
                endYearRun.Append(new Text(endYear.Substring(endYear.Length - 2, 2)));
            }
            else
            {
                startYearRun.Append(new TabChar());
                endYearRun.Append(new TabChar());
            }

            title.Append(startYearPrefixRun, startYearRun, endYearPrefixRun, endYearRun);

            title.Append(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("учебном году")));

            _documentBody.Append(title);
        }

        private void AppendTable(List<StudentsHealthCardRecord> records)
        {
            Table table = new Table();

            TableProperties tblProperties = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }
                )
            );

            table.AppendChild(tblProperties);

            int numberColumnWidth = 225 * 3;
            int studentColumnWidth = 2275 * 3;
            int classesMissedColumnWidth = 1250 * 3;
            int noteColumnWidth = 1250 * 3;

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = numberColumnWidth.ToString() },
                new GridColumn() { Width = studentColumnWidth.ToString() },
                new GridColumn() { Width = classesMissedColumnWidth.ToString() },
                new GridColumn() { Width = noteColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            TableCellProperties cellProperties = new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
            );

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableRow headRow = new TableRow();

            TableCell numberHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("№"))));
            var numberHeadCellProperties = cellProperties.CloneNode(true);
            numberHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = numberColumnWidth.ToString() });
            numberHeadCell.Append(numberHeadCellProperties);

            TableCell studentHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Фамилия, имя, отчество (полностью)"))));
            var studentHeadCellProperties = cellProperties.CloneNode(true);
            studentHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = studentColumnWidth.ToString() });
            studentHeadCell.Append(studentHeadCellProperties);

            TableCell classesMissedHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Пропущено учебных занятий"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("по болезни"))));
            var classesMissedHeadCellProperties = cellProperties.CloneNode(true);
            classesMissedHeadCellProperties.Append(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = classesMissedColumnWidth.ToString() });
            classesMissedHeadCell.Append(classesMissedHeadCellProperties);

            TableCell noteHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Примечание"))));
            var noteHeadCellProperties = cellProperties.CloneNode(true);
            noteHeadCellProperties.Append(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() });
            noteHeadCell.Append(noteHeadCellProperties);

            headRow.Append(numberHeadCell, studentHeadCell, classesMissedHeadCell, noteHeadCell);
            table.Append(headRow);

            foreach (var record in records)
            {
                TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = 540 }));

                Tabs tabs = new Tabs();
                tabs.Append(new TabStop()
                {
                    Val = TabStopValues.Left,
                    Position = 360
                });

                TableCell numberCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Number == null ? "" : ((int)record.Number).ToString()))));
                numberCell.Append(new TableCellProperties(
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = numberColumnWidth.ToString() }));
;
                TableCell studentCell = new TableCell(new Paragraph(
                    new ParagraphProperties(new SpacingBetweenLines { After = "0", Before = "0" }, tabs.CloneNode(true)),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new TabChar(),
                        new Text(record.Student == null ? "" : (GetStudentFIO(record.Student))))));
                studentCell.Append(new TableCellProperties(
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = studentColumnWidth.ToString() }));

                TableCell classesMissedCell = new TableCell(new Paragraph(
                    new ParagraphProperties(new SpacingBetweenLines { After = "0", Before = "0" }, tabs.CloneNode(true)),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new TabChar(),
                        new Text(record.MissedClasses == null ? "" : ((int)record.MissedClasses).ToString()))));
                classesMissedCell.Append(new TableCellProperties(
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = classesMissedColumnWidth.ToString() }));

                TableCell noteCell = new TableCell(new Paragraph(new ParagraphProperties(new SpacingBetweenLines { After = "0", Before = "0" }),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Note == null ? "" : record.Note))));
                noteCell.Append(new TableCellProperties(
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() }));

                row.Append(numberCell, studentCell, classesMissedCell, noteCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }

        private string GetStudentFIO(Student student)
        {
            return student.LastName + " " + student.FirstName + " " + student.MiddleName;
        }
    }
}
