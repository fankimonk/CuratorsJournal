using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages.Attributes;
using Domain.Enums.Journal;
using Frontend.Utils;

namespace Application.Services.Word
{
    public class IdeologicalAndEducationalWorkAccountingPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public IdeologicalAndEducationalWorkAccountingPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.CuratorsIdeologicalAndEducationalWorkAccounting);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                var pageAttributes = page.CuratorsIdeologicalAndEducationalWorkPageAttributes ?? throw new ArgumentException(nameof(page));
                AppendTitle(pageAttributes);

                AppendTable(page.CuratorsIdeologicalAndEducationalWorkAccounting);

                WordUtils.AppendPageBreak(_documentBody);
            }
        }

        private void AppendTitle(CuratorsIdeologicalAndEducationalWorkPageAttributes pageAttributes)
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("УЧЕТ ИДЕОЛОГИЧЕСКОЙ И ВОСПИТАТЕЛЬНОЙ РАБОТЫ КУРАТОРА УЧЕБНОЙ ГРУППЫ"),
                    new Break())
            );

            var monthPrefixRun = new Run(WordUtils.GetRunProperties(bold: true),
                new Text("в "));
            var monthValueRun = new Run(WordUtils.GetRunProperties(underline: true, bold: true));

            monthValueRun.Append(new TabChar());
            if (pageAttributes.Month != null) monthValueRun.Append(
                new Text(MonthsUtils.InMonthsNames[(int)pageAttributes.Month]));
            monthValueRun.Append(new TabChar());

            var yearPrefixRun = new Run(WordUtils.GetRunProperties(bold: true),
                new Text("20"));
            var yearValueRun = new Run(WordUtils.GetRunProperties(underline: true, bold: true));

            var year = pageAttributes.Year;
            if (year != null) yearValueRun.Append(
                new Text(year.ToString().Substring(year.ToString().Length - 2, 2)));
            else yearValueRun.Append(new TabChar());

                var yearPostfixRun = new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("г."));

            title.Append(monthPrefixRun, monthValueRun, yearPrefixRun, yearValueRun, yearPostfixRun);

            _documentBody.Append(title);
        }

        private void AppendTable(List<CuratorsIdeologicalAndEducationalWorkAccountingRecord> records)
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

            int termColumnWidth = 800 * 3;
            int contentColumnWidth = 4200 * 3;

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = termColumnWidth.ToString() },
                new GridColumn() { Width = contentColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            TableRow headRow = new TableRow();

            TableCell termHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Срок выполнения"))));
            termHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = termColumnWidth.ToString() }));

            TableCell contentHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("Содержание работы"))));
            contentHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = contentColumnWidth.ToString() }));

            headRow.Append(termHeadCell, contentHeadCell);
            table.Append(headRow);

            foreach (var record in records)
            {
                TableRow row = new TableRow();

                string termStr = "";
                if (record.StartDate != null && record.EndDate != null)
                {
                    termStr += record.StartDate.ToString() + " - " + record.EndDate.ToString();
                }
                else if (record.StartDate != null && record.EndDate == null)
                {
                    termStr += record.StartDate.ToString();
                }

                TableCell termCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(termStr))));
                termCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = termColumnWidth.ToString() }));

                TableCell contentCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.WorkContent ?? ""))));
                contentCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = contentColumnWidth.ToString() }));

                row.Append(termCell, contentCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }
    }
}
