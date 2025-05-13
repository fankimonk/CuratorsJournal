using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages;
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

        private readonly UInt32Value _valueRowHeight = 370;

        private readonly int _maxRows = 20;

        private readonly int _termColumnWidth = 800 * 3;
        private readonly int _contentColumnWidth = 4200 * 3;

        private readonly TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
        );

        private readonly ParagraphProperties _valueParagraphProperties =
            new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        public IdeologicalAndEducationalWorkAccountingPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.CuratorsIdeologicalAndEducationalWorkAccounting);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                var pageAttributes = page.CuratorsIdeologicalAndEducationalWorkPageAttributes ?? throw new ArgumentException(nameof(page));
                var rows = GenerateRows(page.CuratorsIdeologicalAndEducationalWorkAccounting);
                AppendTable(rows, pageAttributes);
            }
            else
            {
                foreach (var p in pages)
                {
                    var pageAttributes = p.CuratorsIdeologicalAndEducationalWorkPageAttributes ?? throw new ArgumentException(nameof(p));
                    var rows = GenerateRows(p.CuratorsIdeologicalAndEducationalWorkAccounting);
                    AppendTable(rows, pageAttributes);
                }
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

        private List<TableRow> GenerateRows(List<CuratorsIdeologicalAndEducationalWorkAccountingRecord> records)
        {
            var totalRows = new List<TableRow>();
            foreach (var record in records)
            {
                var rows = new List<TableRow>() { new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })) };
                
                string termStr = "";
                if (record.StartDay != null && record.EndDay != null)
                {
                    termStr += record.StartDay.ToString() + " - " + record.EndDay.ToString();
                }
                else if (record.StartDay != null && record.EndDay == null)
                {
                    termStr += record.StartDay.ToString();
                }

                var termCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                termCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _termColumnWidth.ToString() });

                AppendTerm(termStr, rows, termCellProperties);

                var contentStr = record.WorkContent ?? "";
                var contentCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                contentCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _contentColumnWidth.ToString() });

                AppendContent(contentStr, rows, contentCellProperties, termCellProperties);

                totalRows.AddRange(rows);
            }

            return totalRows;
        }

        private void AppendTable(List<TableRow> rows, CuratorsIdeologicalAndEducationalWorkPageAttributes pageAttributes)
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
                new GridColumn() { Width = _termColumnWidth.ToString() },
                new GridColumn() { Width = _contentColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableRow headRow = new TableRow();

            TableCell termHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Срок выполнения"))));
            termHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _termColumnWidth.ToString() }));

            TableCell contentHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Содержание работы"))));
            contentHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _contentColumnWidth.ToString() }));

            headRow.Append(termHeadCell, contentHeadCell);
            table.Append(headRow);

            int pageCount = rows.Count / _maxRows;
            pageCount += rows.Count % _maxRows == 0 ? 0 : 1;
            for (int i = 0; i < pageCount; i++)
            {
                var currentTable = table.CloneNode(true);
                AppendTitle(pageAttributes);
                var currentPageRows = rows.Skip(i * _maxRows).Take(_maxRows);
                foreach (var row in currentPageRows)
                    currentTable.Append(row);
                for (int j = currentPageRows.Count(); j < _maxRows; j++)
                    AppendEmptyRow((Table)currentTable);
                _documentBody.Append(currentTable);
                AppendSignature();
                WordUtils.AppendSectionBreak(WordUtils.PageOrientationTypes.Landscape, _documentBody);
            }
        }

        private void AppendSignature()
        {
            var paragraphProperties = new ParagraphProperties(new SpacingBetweenLines() { Before = "0", After = "0" });

            var signatureParagraphProperties = paragraphProperties.CloneNode(true);
            signatureParagraphProperties.Append(new Justification { Val = JustificationValues.Both });
            Tabs tabs = new Tabs();
            tabs.Append(new TabStop()
            {
                Val = TabStopValues.Left,
                Position = 360
            });
            signatureParagraphProperties.Append(tabs);

            var labelRun = new Run(WordUtils.GetRunProperties(fontSize: "26"),
                new TabChar(),
                new Text("Декан (Заместитель декана по воспитательной работе)"));
            for (int i = 0; i < 5; i++)
                labelRun.Append(new TabChar());

            var signatureRun = new Run(WordUtils.GetRunProperties(fontSize: "26", underline: true));
            for (int i = 0; i < 5; i++)
                signatureRun.Append(new TabChar());

            var signatureParagraph = new Paragraph(signatureParagraphProperties, labelRun, signatureRun);

            var signatureLabelParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var signatureLabelRun = new Run(WordUtils.GetRunProperties(fontSize: "20"));
            for (int i = 0; i < 15; i++)
                signatureLabelRun.Append(new TabChar());
            signatureLabelRun.Append(new Text("Дата подпись"));
            signatureLabelParagraph.Append(signatureLabelRun);

            _documentBody.Append(signatureParagraph, signatureLabelParagraph);
        }

        private void AppendEmptyRow(Table table)
        {
            TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

            var termCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            termCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _termColumnWidth.ToString() });
            TableCell termCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text(""))));
            termCell.Append(termCellProperties);

            var contentCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            contentCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _contentColumnWidth.ToString() });
            TableCell contentCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text(""))));
            contentCell.Append(contentCellProperties);

            row.Append(termCell, contentCell);
            table.Append(row);
        }

        private void AppendTerm(string str, List<TableRow> rows, TableCellProperties cellProperties)
        {
            var splitTerm = str.Split(' ');
            if (splitTerm.Length > 2)
            {
                WordUtils.AddCellToRow(rows[0], splitTerm[0] + " " + splitTerm[1] + " ", cellProperties, _valueParagraphProperties);
                rows.Add(new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })));
                WordUtils.AddCellToRow(rows[1], splitTerm[2], cellProperties, _valueParagraphProperties);
            }
            else
            {
                WordUtils.AddCellToRow(rows[0], str, cellProperties, _valueParagraphProperties);
            }
        }

        private void AppendContent(string contentStr, List<TableRow> rows, TableCellProperties contentCellProperties, TableCellProperties termCellProperties)
        {
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", termCellProperties, _valueParagraphProperties);
                    }
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, contentCellProperties, _valueParagraphProperties);
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
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", termCellProperties, _valueParagraphProperties);
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, contentCellProperties, _valueParagraphProperties);
                currentRowIndex++;
            }

            for (int i = currentRowIndex; i < rows.Count; i++)
            {
                WordUtils.AddCellToRow(rows[i], "", contentCellProperties, _valueParagraphProperties);
            }
        }
    }
}
