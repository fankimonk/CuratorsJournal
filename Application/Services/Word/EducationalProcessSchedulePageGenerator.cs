using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class EducationalProcessSchedulePageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public EducationalProcessSchedulePageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.EducationalProcessSchedule);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                AppendTitle();

                AppendTable(page.EducationalProcessSchedule);

                WordUtils.AppendPageBreak(_documentBody);
            }
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

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = "1250" },
                new GridColumn() { Width = "1350" },
                new GridColumn() { Width = "1600" },
                new GridColumn() { Width = "2000" },
                new GridColumn() { Width = "2200" },
                new GridColumn() { Width = "1600" }
            );
            table.AppendChild(tableGrid);

            TableRow headRow = new TableRow();

            TableCell numberHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Семестр"))));
            numberHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1250" }));

            TableCell startHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("Начало"))));
            startHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1350" }));

            TableCell endHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("Окончание"))));
            endHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1600" }));

            TableCell sessionHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Сессия"))));
            sessionHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2000" }));

            TableCell practiceHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("Практика"))));
            practiceHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2200" }));

            TableCell vacationHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("Каникулы"))));
            vacationHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1600" }));

            headRow.Append(numberHeadCell, startHeadCell, endHeadCell, sessionHeadCell, practiceHeadCell, vacationHeadCell);
            table.Append(headRow);

            foreach (var record in schedule)
            {
                TableRow row = new TableRow();

                TableCell numberCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.SemesterNumber == null ? "" : ((int)record.SemesterNumber).ToString()))));
                numberCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1250" }));

                TableCell startCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.StartDate == null ? "" : ((DateOnly)record.StartDate).ToString()))));
                startCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1350" }));

                TableCell endCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.EndDate == null ? "" : ((DateOnly)record.EndDate).ToString()))));
                endCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1600" }));

                TableCell sessionCell = new TableCell();
                var sessionCellParagraph = new Paragraph();
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

                sessionCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2000" }));

                TableCell practiceCell = new TableCell();
                var practiceCellParagraph = new Paragraph();
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

                practiceCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2200" }));

                TableCell vacationCell = new TableCell();
                var vacationCellParagraph = new Paragraph();
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

                vacationCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1600" }));

                row.Append(numberCell, startCell, endCell, sessionCell, practiceCell, vacationCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }
    }
}
