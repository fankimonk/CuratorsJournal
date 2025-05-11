using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class RecommendationsAndRemarksPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
        );

        private ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        private UInt32Value _valueRowHeight = 440;

        public RecommendationsAndRemarksPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.RecomendationsAndRemarks);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                AppendTitle();

                AppendTable(page.RecomendationsAndRemarks);

                WordUtils.AppendPageBreak(_documentBody);
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

        private void AppendTable(List<RecomendationsAndRemarksRecord> records)
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

            int dateColumnWidth = 375 * 3;
            int reviewerColumnWidth = 1280 * 3;
            int contentColumnWidth = 2490 * 3;
            int resultColumnWidth = 855 * 3;

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = dateColumnWidth.ToString() },
                new GridColumn() { Width = reviewerColumnWidth.ToString() },
                new GridColumn() { Width = contentColumnWidth.ToString() },
                new GridColumn() { Width = resultColumnWidth.ToString() }
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
            dateHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() });
            dateHeadCell.Append(dateHeadCellProperties);

            TableCell reviewerHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Кто проверил"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("(должность, фамилия)"))));
            var reviewerHeadCellProperties = cellProperties.CloneNode(true);
            reviewerHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = reviewerColumnWidth.ToString() });
            reviewerHeadCell.Append(reviewerHeadCellProperties);

            TableCell contentHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Рекомендации, замечания"))));
            var contentHeadCellProperties = cellProperties.CloneNode(true);
            contentHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = contentColumnWidth.ToString() });
            contentHeadCell.Append(contentHeadCellProperties);

            TableCell resultHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Выполнение,"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата"))));
            var resultHeadCellProperties = cellProperties.CloneNode(true);
            resultHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = resultColumnWidth.ToString() });
            resultHeadCell.Append(resultHeadCellProperties);

            headRow.Append(dateHeadCell, reviewerHeadCell, contentHeadCell, resultHeadCell);
            table.Append(headRow);

            foreach (var record in records)
            {
                var rows = new List<TableRow>() { new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })) };
                var dateCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() });
                TableCell dateCell = new TableCell(
                    new Paragraph(_valueParagraphProperties.CloneNode(true),
                        new Run(WordUtils.GetRunProperties(fontSize: "24"),
                            new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(dateCellProperties);
                rows[0].Append(dateCell);

                var reviewerStr = record.Reviewer == null ? "" : GetReviewerString(record.Reviewer);
                var reviewerCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                reviewerCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = reviewerColumnWidth.ToString() });
                //TableCell reviewerCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                //    new Text(record.Reviewer == null ? "" : GetReviewerString(record.Reviewer)))));
                //reviewerCell.Append(reviewerCellProperties);
                AppendReviewer(reviewerStr, rows, reviewerCellProperties, dateCellProperties, table);

                var contentStr = record.Content ?? "";
                var contentCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                contentCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = contentColumnWidth.ToString() });
                //TableCell contentCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                //    new Text(record.Content == null ? "" : record.Content))));
                //contentCell.Append(contentCellProperties);
                AppendContent(contentStr, rows, contentCellProperties, reviewerCellProperties, dateCellProperties, table);

                string resultStr = "";
                if (record.Result != null) resultStr += record.Result + ", ";
                if (record.ExecutionDate != null) resultStr += record.ExecutionDate.ToString();

                var resultCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                resultCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = resultColumnWidth.ToString() });
                //TableCell resultCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                //    new Text(resultStr))));
                //resultCell.Append(resultCellProperties);
                AppendResult(resultStr, rows, resultCellProperties, contentCellProperties, reviewerCellProperties, dateCellProperties, table);

                foreach (var row in rows) table.Append(row);
            }

            _documentBody.Append(table);
        }

        private string GetReviewerString(Worker worker)
        {
            string positionStr = "";
            if (worker.Position != null) positionStr += worker.Position.Name + ", ";
            return positionStr + worker.LastName + " " + worker.FirstName[0] + ". " + worker.MiddleName[0] + ".";
        }

        private void AppendReviewer(string str, List<TableRow> rows, TableCellProperties reviewerCellProperties,
            TableCellProperties dateCellProperties, Table table)
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

            //for (int i = currentRowIndex; i < rows.Count; i++)
            //{
            //    WordUtils.AddCellToRow(rows[i], "", reviewerCellProperties, _valueParagraphProperties, "24");
            //}
        }

        private void AppendContent(string str, List<TableRow> rows, TableCellProperties contentCellProperties, TableCellProperties reviewerCellProperties,
            TableCellProperties dateCellProperties, Table table)
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
            TableCellProperties reviewerCellProperties, TableCellProperties dateCellProperties, Table table)
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
