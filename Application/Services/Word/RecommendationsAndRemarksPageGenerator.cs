using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class RecommendationsAndRemarksPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private readonly TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
        );

        private readonly ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        private readonly UInt32Value _valueRowHeight = 410;

        private readonly int _maxRows = 20;

        private readonly int _dateColumnWidth = 375 * 3;
        private readonly int _reviewerColumnWidth = 1280 * 3;
        private readonly int _contentColumnWidth = 2490 * 3;
        private readonly int _resultColumnWidth = 855 * 3;

        public RecommendationsAndRemarksPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.RecomendationsAndRemarks);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                var rows = GenerateRows(page.RecomendationsAndRemarks);
                AppendTable(rows);
            }
            else
            {
                foreach (var p in pages)
                {
                    var rows = GenerateRows(p.RecomendationsAndRemarks);
                    AppendTable(rows);
                }
            } 
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("РЕКОМЕНДАЦИИ И ЗАМЕЧАНИЯ ЛИЦ, ПРОВЕРЯЮЩИХ РАБОТУ КУРАТОРА"))
            );

            _documentBody.Append(title);
        }

        private List<TableRow> GenerateRows(List<RecomendationsAndRemarksRecord> records)
        {
            var totalRows = new List<TableRow>();
            foreach (var record in records)
            {
                var rows = new List<TableRow>() { new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })) };
                var dateCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() });
                TableCell dateCell = new TableCell(
                    new Paragraph(_valueParagraphProperties.CloneNode(true),
                        new Run(WordUtils.GetRunProperties(fontSize: "24"),
                            new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(dateCellProperties);
                rows[0].Append(dateCell);

                var reviewerStr = record.Reviewer == null ? "" : GetReviewerString(record.Reviewer);
                var reviewerCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                reviewerCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _reviewerColumnWidth.ToString() });
                AppendReviewer(reviewerStr, rows, reviewerCellProperties, dateCellProperties);

                var contentStr = record.Content ?? "";
                var contentCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                contentCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _contentColumnWidth.ToString() });
                AppendContent(contentStr, rows, contentCellProperties, reviewerCellProperties, dateCellProperties);

                string resultStr = "";
                if (record.Result != null) resultStr += record.Result + ", ";
                if (record.ExecutionDate != null) resultStr += record.ExecutionDate.ToString();

                var resultCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                resultCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _resultColumnWidth.ToString() });
                AppendResult(resultStr, rows, resultCellProperties, contentCellProperties, reviewerCellProperties, dateCellProperties);

                totalRows.AddRange(rows);
            }

            return totalRows;
        }

        private void AppendTable(List<TableRow> rows)
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
                new GridColumn() { Width = _dateColumnWidth.ToString() },
                new GridColumn() { Width = _reviewerColumnWidth.ToString() },
                new GridColumn() { Width = _contentColumnWidth.ToString() },
                new GridColumn() { Width = _resultColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableCellProperties cellProperties = new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
            );

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата"))));
            var dateHeadCellProperties = cellProperties.CloneNode(true);
            dateHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() });
            dateHeadCell.Append(dateHeadCellProperties);

            TableCell reviewerHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Кто проверил"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("(должность, фамилия)"))));
            var reviewerHeadCellProperties = cellProperties.CloneNode(true);
            reviewerHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _reviewerColumnWidth.ToString() });
            reviewerHeadCell.Append(reviewerHeadCellProperties);

            TableCell contentHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Рекомендации, замечания"))));
            var contentHeadCellProperties = cellProperties.CloneNode(true);
            contentHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _contentColumnWidth.ToString() });
            contentHeadCell.Append(contentHeadCellProperties);

            TableCell resultHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Выполнение,"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата"))));
            var resultHeadCellProperties = cellProperties.CloneNode(true);
            resultHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _resultColumnWidth.ToString() });
            resultHeadCell.Append(resultHeadCellProperties);

            headRow.Append(dateHeadCell, reviewerHeadCell, contentHeadCell, resultHeadCell);
            table.Append(headRow);

            var dateCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() });
            var reviewerCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            reviewerCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _reviewerColumnWidth.ToString() });
            var contentCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            contentCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _contentColumnWidth.ToString() });
            var resultCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            resultCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _resultColumnWidth.ToString() });

            int pageCount;
            if (rows.Count == 0) pageCount = 1;
            else
            {
                pageCount = rows.Count / _maxRows;
                pageCount += rows.Count % _maxRows == 0 ? 0 : 1;
            }
            for (int i = 0; i < pageCount; i++)
            {
                var currentTable = table.CloneNode(true);
                AppendTitle();
                var currentPageRows = rows.Skip(i * _maxRows).Take(_maxRows);
                foreach (var row in currentPageRows)
                    currentTable.Append(row);
                for (int j = currentPageRows.Count(); j < _maxRows; j++)
                    AppendEmptyRow((Table)currentTable, dateCellProperties, reviewerCellProperties, contentCellProperties, resultCellProperties);
                _documentBody.Append(currentTable);
                WordUtils.AppendSectionBreak(WordUtils.PageOrientationTypes.Landscape, _documentBody);
            }
        }

        private void AppendEmptyRow(Table table, TableCellProperties dateCellProperties, TableCellProperties reviewerCellProperties,
            TableCellProperties contentCellProperties, TableCellProperties resultCellProperties)
        {
            var row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

            TableCell dateCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            dateCell.Append(dateCellProperties.CloneNode(true));

            TableCell reviewerCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            reviewerCell.Append(reviewerCellProperties.CloneNode(true));

            TableCell contentCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            contentCell.Append(contentCellProperties.CloneNode(true));

            TableCell resultCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            resultCell.Append(resultCellProperties.CloneNode(true));

            row.Append(dateCell, reviewerCell, contentCell, resultCell);
            table.Append(row);
        }

        private string GetReviewerString(Worker worker)
        {
            string positionStr = "";
            if (worker.Position != null) positionStr += worker.Position.Name + ", ";
            return positionStr + worker.LastName + " " + worker.FirstName[0] + ". " + worker.MiddleName[0] + ".";
        }

        private void AppendReviewer(string str, List<TableRow> rows, TableCellProperties reviewerCellProperties,
            TableCellProperties dateCellProperties)
        {
            var split = str.Split(' ');
            var lines = new List<string>();
            string currentLine = "";
            int lineMaxChars = 30;
            int currentRowIndex = 0;
            foreach (var word in split)
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                    }
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, reviewerCellProperties, _valueParagraphProperties, "24");
                    lines.Add(currentLine);
                    currentRowIndex++;
                    currentLine = word + " ";
                }
            }

            if (!string.IsNullOrEmpty(currentLine) && !lines.Contains(currentLine))
            {
                if (currentRowIndex == rows.Count)
                {
                    rows.Add(new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })));
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, reviewerCellProperties, _valueParagraphProperties, "24");
                currentRowIndex++;
            }
        }

        private void AppendContent(string str, List<TableRow> rows, TableCellProperties contentCellProperties, TableCellProperties reviewerCellProperties,
            TableCellProperties dateCellProperties)
        {
            var split = str.Split(' ');
            var lines = new List<string>();
            string currentLine = "";
            int lineMaxChars = 60;
            int currentRowIndex = 0;
            foreach (var word in split)
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", reviewerCellProperties, _valueParagraphProperties, "24");
                    }
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, contentCellProperties, _valueParagraphProperties, "24");
                    lines.Add(currentLine);
                    currentRowIndex++;
                    currentLine = word + " ";
                }
            }

            if (!string.IsNullOrEmpty(currentLine) && !lines.Contains(currentLine))
            {
                if (currentRowIndex == rows.Count)
                {
                    rows.Add(new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })));
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", reviewerCellProperties, _valueParagraphProperties, "24");
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, contentCellProperties, _valueParagraphProperties, "24");
                currentRowIndex++;
            }

            for (int i = currentRowIndex; i < rows.Count; i++)
            {
                WordUtils.AddCellToRow(rows[i], "", contentCellProperties, _valueParagraphProperties, "24");
            }
        }

        private void AppendResult(string str, List<TableRow> rows, TableCellProperties resultCellProperties, TableCellProperties contentCellProperties, 
            TableCellProperties reviewerCellProperties, TableCellProperties dateCellProperties)
        {
            var split = str.Split(' ');
            var lines = new List<string>();
            string currentLine = "";
            int lineMaxChars = 18;
            int currentRowIndex = 0;
            foreach (var word in split)
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", reviewerCellProperties, _valueParagraphProperties, "24");
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", contentCellProperties, _valueParagraphProperties, "24");
                    }
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, resultCellProperties, _valueParagraphProperties, "24");
                    lines.Add(currentLine);
                    currentRowIndex++;
                    currentLine = word + " ";
                }
            }

            if (!string.IsNullOrEmpty(currentLine) && !lines.Contains(currentLine))
            {
                if (currentRowIndex == rows.Count)
                {
                    rows.Add(new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })));
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", reviewerCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", contentCellProperties, _valueParagraphProperties, "24");
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, resultCellProperties, _valueParagraphProperties, "24");
                currentRowIndex++;
            }

            for (int i = currentRowIndex; i < rows.Count; i++)
            {
                WordUtils.AddCellToRow(rows[i], "", resultCellProperties, _valueParagraphProperties, "24");
            }
        }
    }
}
