using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class EducationalProcessSchedulePageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private readonly int _minRows = 6;
        private readonly int _maxRows = 8;

        private ParagraphProperties _paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

        public EducationalProcessSchedulePageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.EducationalProcessSchedule);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var p in pages)
            {
                GeneratePage(p);
                int recordsCount = p.EducationalProcessSchedule.Count;
                int rowsCount = recordsCount < _minRows ? _minRows : recordsCount > _maxRows ? _maxRows : recordsCount;
                int breaksCount = 2 + 2 * (_maxRows - rowsCount);
                WordUtils.AppendBreaks(breaksCount, _documentBody);
            }
        }

        private void GeneratePage(Page page)
        {
            AppendTitle();

            AppendTable(page.EducationalProcessSchedule);
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(
                    WordUtils.GetRunProperties(bold:true),
                    new Text("ГРАФИК УЧЕБНОГО ПРОЦЕССА"))
            );

            _documentBody.Append(title);
        }

        private void AppendTable(List<EducationalProcessScheduleRecord> schedule)
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

            TableCellProperties cellProperties = new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
            );

            TableCellMargin cellMargin = new TableCellMargin(
                new TopMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
                new BottomMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
                new LeftMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
                new RightMargin { Width = "100", Type = TableWidthUnitValues.Dxa }
            );

            //cellProperties.Append(cellMargin);

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = "1250" },
                new GridColumn() { Width = "1350" },
                new GridColumn() { Width = "1600" },
                new GridColumn() { Width = "2000" },
                new GridColumn() { Width = "2200" },
                new GridColumn() { Width = "1600" }
            );
            table.AppendChild(tableGrid);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableRow headRow = new TableRow();

            TableCell numberHeadCell = new TableCell(new Paragraph(
                paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Семестр"))));
            var numberHeadCellProperties = cellProperties.CloneNode(true);
            numberHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1250" });
            numberHeadCell.Append(numberHeadCellProperties);

            TableCell startHeadCell = new TableCell(new Paragraph(
                paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Начало"))));
            var startHeadCellProperties = cellProperties.CloneNode(true);
            startHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1350" });
            startHeadCell.Append(startHeadCellProperties);

            TableCell endHeadCell = new TableCell(new Paragraph(
                paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Окончание"))));
            var endHeadCellProperties = cellProperties.CloneNode(true);
            endHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1600" });
            endHeadCell.Append(endHeadCellProperties);

            TableCell sessionHeadCell = new TableCell(new Paragraph(
                paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Сессия"))));
            var sessionHeadCellProperties = cellProperties.CloneNode(true);
            sessionHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2000" });
            sessionHeadCell.Append(sessionHeadCellProperties);

            TableCell practiceHeadCell = new TableCell(new Paragraph(
                paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Практика"))));
            var practiceHeadCellProperties = cellProperties.CloneNode(true);
            practiceHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2200" });
            practiceHeadCell.Append(practiceHeadCellProperties);

            TableCell vacationHeadCell = new TableCell(new Paragraph(
                paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Каникулы"))));
            var vacationHeadCellProperties = cellProperties.CloneNode(true);
            vacationHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1600" });
            vacationHeadCell.Append(vacationHeadCellProperties);

            headRow.Append(numberHeadCell, startHeadCell, endHeadCell, sessionHeadCell, practiceHeadCell, vacationHeadCell);
            table.Append(headRow);

            if (schedule.Count > _maxRows) schedule = schedule.Take(_maxRows).ToList();
            foreach (var record in schedule)
            {
                TableRow row = new TableRow();
                var rowProperties = new TableRowProperties();
                rowProperties.Append(new TableRowHeight { Val = 760 });
                row.AppendChild(rowProperties);

                TableCell numberCell = new TableCell(new Paragraph(
                    paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.SemesterNumber == null ? "" : ((int)record.SemesterNumber).ToString()))));
                numberCell.Append(numberHeadCellProperties.CloneNode(true));

                TableCell startCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.StartDate == null ? "" : ((DateOnly)record.StartDate).ToString()))));
                startCell.Append(startHeadCellProperties.CloneNode(true));

                TableCell endCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.EndDate == null ? "" : ((DateOnly)record.EndDate).ToString()))));
                endCell.Append(endHeadCellProperties.CloneNode(true));

                TableCell sessionCell = new TableCell();
                var sessionCellParagraph = new Paragraph(paragraphProperties.CloneNode(true));
                var sessionStartDateRun = new Run(WordUtils.GetRunProperties(fontSize: "24"));
                var sessionEndDateRun = new Run(WordUtils.GetRunProperties(fontSize: "24"));

                if (record.SessionStartDate == null || record.SessionEndDate == null)
                {
                    sessionStartDateRun.Append(new Text(""));
                    sessionEndDateRun.Append(new Text(""));
                }
                else
                {
                    sessionStartDateRun.Append(new Text("с " + record.SessionStartDate.ToString() + " -"));
                    sessionStartDateRun.Append(new Break());
                    sessionEndDateRun.Append(new Text("по " + record.SessionEndDate.ToString()));
                }

                sessionCellParagraph.Append(sessionStartDateRun);
                sessionCellParagraph.Append(sessionEndDateRun);
                sessionCell.Append(sessionCellParagraph);

                sessionCell.Append(sessionHeadCellProperties.CloneNode(true));

                TableCell practiceCell = new TableCell();
                var practiceCellParagraph = new Paragraph(paragraphProperties.CloneNode(true));
                var practiceStartDateRun = new Run(WordUtils.GetRunProperties(fontSize: "24"));
                var practiceEndDateRun = new Run(WordUtils.GetRunProperties(fontSize: "24"));

                if (record.PracticeStartDate == null || record.PracticeEndDate == null)
                {
                    practiceStartDateRun.Append(new Text(""));
                    practiceEndDateRun.Append(new Text(""));
                }
                else
                {
                    practiceStartDateRun.Append(new Text("с " + record.PracticeStartDate.ToString() + " -"));
                    practiceStartDateRun.Append(new Break());
                    practiceEndDateRun.Append(new Text("по " + record.PracticeEndDate.ToString()));
                }

                practiceCellParagraph.Append(practiceStartDateRun);
                practiceCellParagraph.Append(practiceEndDateRun);
                practiceCell.Append(practiceCellParagraph);

                practiceCell.Append(practiceHeadCellProperties.CloneNode(true));

                TableCell vacationCell = new TableCell();
                var vacationCellParagraph = new Paragraph(paragraphProperties.CloneNode(true));
                var vacationStartDateRun = new Run(WordUtils.GetRunProperties(fontSize: "24"));
                var vacationEndDateRun = new Run(WordUtils.GetRunProperties(fontSize: "24"));

                if (record.VacationStartDate == null || record.VacationEndDate == null)
                {
                    vacationStartDateRun.Append(new Text(""));
                    vacationEndDateRun.Append(new Text(""));
                }
                else
                {
                    vacationStartDateRun.Append(new Text("с " + record.VacationStartDate.ToString() + " -"));
                    vacationStartDateRun.Append(new Break());
                    vacationEndDateRun.Append(new Text("по " + record.VacationEndDate.ToString()));
                }

                vacationCellParagraph.Append(vacationStartDateRun);
                vacationCellParagraph.Append(vacationEndDateRun);
                vacationCell.Append(vacationCellParagraph);

                vacationCell.Append(vacationHeadCellProperties.CloneNode(true));

                row.Append(numberCell, startCell, endCell, sessionCell, practiceCell, vacationCell);
                table.Append(row);
            }

            for (int i = schedule.Count; i < _minRows; i++)
                AppendEmptyRow(table, (TableCellProperties)numberHeadCellProperties, (TableCellProperties)startHeadCellProperties, (TableCellProperties)endHeadCellProperties,
                    (TableCellProperties)sessionHeadCellProperties, (TableCellProperties)practiceHeadCellProperties, (TableCellProperties)vacationHeadCellProperties);

            _documentBody.Append(table);
        }

        private void AppendEmptyRow(Table table, TableCellProperties numberCellProperties, TableCellProperties startCellProperties,
            TableCellProperties endCellProperties, TableCellProperties sessionCellProperties, TableCellProperties practiceCellProperties,
            TableCellProperties vacationCellProperties)
        {
            TableRow row = new TableRow();
            var rowProperties = new TableRowProperties();
            rowProperties.Append(new TableRowHeight { Val = 760 });
            row.AppendChild(rowProperties);

            TableCell numberCell = new TableCell(new Paragraph(
                _paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            numberCell.Append(numberCellProperties.CloneNode(true));

            TableCell startCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true), new Run(WordUtils.GetRunProperties(fontSize: "24"),
                new Text(""))));
            startCell.Append(startCellProperties.CloneNode(true));

            TableCell endCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true), new Run(WordUtils.GetRunProperties(fontSize: "24"),
                new Text(""))));
            endCell.Append(endCellProperties.CloneNode(true));

            TableCell sessionCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true), new Run(WordUtils.GetRunProperties(fontSize: "24"),
                new Text(""))));
            endCell.Append(sessionCellProperties.CloneNode(true));

            TableCell practiceCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true), new Run(WordUtils.GetRunProperties(fontSize: "24"),
                new Text(""))));
            endCell.Append(practiceCellProperties.CloneNode(true));

            TableCell vacationCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true), new Run(WordUtils.GetRunProperties(fontSize: "24"),
                new Text(""))));
            endCell.Append(vacationCellProperties.CloneNode(true));

            row.Append(numberCell, startCell, endCell, sessionCell, practiceCell, vacationCell);
            table.Append(row);
        }
    }
}
