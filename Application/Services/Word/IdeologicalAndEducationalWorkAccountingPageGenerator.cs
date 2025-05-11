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

        private UInt32Value _valueRowHeight = 360;

        public IdeologicalAndEducationalWorkAccountingPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.CuratorsIdeologicalAndEducationalWorkAccounting);
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

            TableCellProperties cellProperties = new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
            );

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableRow headRow = new TableRow();

            TableCell termHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Срок выполнения"))));
            termHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = termColumnWidth.ToString() }));

            TableCell contentHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Содержание работы"))));
            contentHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = contentColumnWidth.ToString() }));

            headRow.Append(termHeadCell, contentHeadCell);
            table.Append(headRow);

            var valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

            foreach (var record in records)
            {
                var rows = new List<TableRow>() { new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })) };
                
                string termStr = "";
                if (record.StartDate != null && record.EndDate != null)
                {
                    termStr += record.StartDate.ToString() + " - " + record.EndDate.ToString();
                }
                else if (record.StartDate != null && record.EndDate == null)
                {
                    termStr += record.StartDate.ToString();
                }

                var termCellProperties = (TableCellProperties)cellProperties.CloneNode(true);
                termCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = termColumnWidth.ToString() });

                var splitTerm = termStr.Split(' ');
                if (splitTerm.Length > 2)
                {
                    AddCellToRow(rows[0], splitTerm[0] + " " + splitTerm[1] + " ", termCellProperties, valueParagraphProperties);
                    rows.Add(new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })));
                    AddCellToRow(rows[1], splitTerm[2], termCellProperties, valueParagraphProperties);
                }
                else
                {
                    AddCellToRow(rows[0], termStr, termCellProperties, valueParagraphProperties);
                }

                var contentStr = record.WorkContent ?? "";
                var contentCellProperties = (TableCellProperties)cellProperties.CloneNode(true);
                contentCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = contentColumnWidth.ToString() });

                var splitContent = contentStr.Split(' ');
                var contentLines = new List<string>();
                string currentLine = "";
                int lineMaxChars = 90;
                int currentRowIndex = 0;
                foreach (var word in splitContent)
                {
                    if (currentLine.Length + word.Length + 1 <= lineMaxChars)
                    {
                        currentLine += word + " ";
                    }
                    else
                    {
                        if (currentRowIndex == rows.Count)
                        {
                            rows.Add(new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })));
                            AddCellToRow(rows[currentRowIndex], "", termCellProperties, valueParagraphProperties);
                        }
                        AddCellToRow(rows[currentRowIndex], currentLine, contentCellProperties, valueParagraphProperties);
                        contentLines.Add(currentLine);
                        currentRowIndex++;
                        currentLine = word + " ";
                    }
                }

                if (!string.IsNullOrEmpty(currentLine) && !contentLines.Contains(currentLine))
                {
                    if (currentRowIndex == rows.Count)
                    {
                        rows.Add(new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })));
                        AddCellToRow(rows[currentRowIndex], "", termCellProperties, valueParagraphProperties);
                    }
                    AddCellToRow(rows[currentRowIndex], currentLine, contentCellProperties, valueParagraphProperties);
                    currentRowIndex++;
                }

                for (int i = currentRowIndex; i < rows.Count; i++)
                {
                    AddCellToRow(rows[i], "", contentCellProperties, valueParagraphProperties);
                }

                foreach (var row in rows) table.Append(row);
            }

            _documentBody.Append(table);
        }

        private void AddCellToRow(TableRow row, string content, TableCellProperties? cellProperties = null, ParagraphProperties? paragraphProperties = null)
        {
            TableCell cell = new TableCell();
            if (cellProperties != null) cell.Append(cellProperties.CloneNode(true));
            var paragraph = new Paragraph();
            if (paragraphProperties != null) paragraph.Append(paragraphProperties.CloneNode(true));
            paragraph.Append(new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text(content)));
            cell.Append(paragraph);
            row.Append(cell);
        }
    }
}
