using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class InformationHoursAccountingPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private readonly int _journalId;

        private readonly TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
        );

        private readonly ParagraphProperties _paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
            new SpacingBetweenLines { Before = "0", After = "0" });
        private readonly ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        private readonly UInt32Value _valueRowHeight = 420;

        private readonly int _maxRows = 20;

        private readonly int _dateColumnWidth = 875 * 3;
        private readonly int _topicColumnWidth = 1625 * 3;
        private readonly int _noteColumnWidth = 2500 * 3;

        public InformationHoursAccountingPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.InformationHoursAccounting);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                var rows = GenerateRows(page.InformationHoursAccounting);
                AppendTable(rows);
            }
            else
            {
                foreach (var p in pages)
                {
                    var rows = GenerateRows(p.InformationHoursAccounting);
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
                    new Text("УЧЕТ ИНФОРМАЦИОННЫХ ЧАСОВ"))
            );

            _documentBody.Append(title);
        }

        private List<TableRow> GenerateRows(List<InformationHoursAccountingRecord> records)
        {
            var totalRows = new List<TableRow>();
            foreach (var record in records)
            {
                var rows = new List<TableRow>() { new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })) };

                var dateCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() });

                TableCell dateCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "26"),
                        new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(dateCellProperties);
                rows[0].Append(dateCell);

                var topicStr = record.Topic ?? "";
                var topicCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                topicCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _topicColumnWidth.ToString() });

                AppendTopic(topicStr, rows, topicCellProperties, dateCellProperties);

                var noteStr = record.Note ?? "";
                var noteCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                noteCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _noteColumnWidth.ToString() });

                AppendNote(noteStr, rows, noteCellProperties, topicCellProperties, dateCellProperties);

                totalRows.AddRange(rows);
            }

            return totalRows;
        }

        private void AppendEmptyRow(Table table)
        {
            var row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

            var dateCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() });
            TableCell dateCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text(""))));
            dateCell.Append(dateCellProperties);

            var topicCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            topicCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _topicColumnWidth.ToString() });
            TableCell topicCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text(""))));
            topicCell.Append(topicCellProperties);

            var noteCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            noteCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _noteColumnWidth.ToString() });
            TableCell noteCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text(""))));
            noteCell.Append(noteCellProperties);

            row.Append(dateCell, topicCell, noteCell);
            table.Append(row);
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
                new GridColumn() { Width = _topicColumnWidth.ToString() },
                new GridColumn() { Width = _noteColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата проведения"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() }));

            TableCell topicHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Тема"))));
            topicHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _topicColumnWidth.ToString() }));

            TableCell noteHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Примечание"))));
            noteHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _noteColumnWidth.ToString() }));

            headRow.Append(dateHeadCell, topicHeadCell, noteHeadCell);
            table.Append(headRow);

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
                    AppendEmptyRow((Table)currentTable);
                _documentBody.Append(currentTable);
                WordUtils.AppendSectionBreak(WordUtils.PageOrientationTypes.Landscape, _documentBody);
            }
        }

        private void AppendTopic(string topicStr, List<TableRow> rows, TableCellProperties topicCellProperties, 
            TableCellProperties dateCellProperties)
        {
            var splitTopic = topicStr.Split(' ');
            var topicLines = new List<string>();
            string currentTopicLine = "";
            int topicLineMaxChars = 35;
            int currentTopicRowIndex = 0;
            foreach (var word in splitTopic)
            {
                if (currentTopicLine.Length + word.Length + 1 <= topicLineMaxChars)
                {
                    currentTopicLine += word + " ";
                }
                else
                {
                    if (currentTopicRowIndex == rows.Count)
                    {
                        rows.Add(new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })));
                        WordUtils.AddCellToRow(rows[currentTopicRowIndex], "", dateCellProperties, _valueParagraphProperties);
                    }
                    WordUtils.AddCellToRow(rows[currentTopicRowIndex], currentTopicLine, topicCellProperties, _valueParagraphProperties);
                    topicLines.Add(currentTopicLine);
                    currentTopicRowIndex++;
                    currentTopicLine = word + " ";
                }
            }

            if (!string.IsNullOrEmpty(currentTopicLine) && !topicLines.Contains(currentTopicLine))
            {
                if (currentTopicRowIndex == rows.Count)
                {
                    rows.Add(new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })));
                    WordUtils.AddCellToRow(rows[currentTopicRowIndex], "", dateCellProperties, _valueParagraphProperties);
                }
                WordUtils.AddCellToRow(rows[currentTopicRowIndex], currentTopicLine, topicCellProperties, _valueParagraphProperties);
                currentTopicRowIndex++;
            }

            for (int i = currentTopicRowIndex; i < rows.Count; i++)
            {
                WordUtils.AddCellToRow(rows[i], "", topicCellProperties, _valueParagraphProperties);
            }
        }

        private void AppendNote(string noteStr, List<TableRow> rows, TableCellProperties noteCellProperties,
            TableCellProperties topicCellProperties, TableCellProperties dateCellProperties)
        {
            var split = noteStr.Split(' ');
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties);
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", topicCellProperties, _valueParagraphProperties);
                    }
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, noteCellProperties, _valueParagraphProperties);
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
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties);
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", topicCellProperties, _valueParagraphProperties);
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, noteCellProperties, _valueParagraphProperties);
                currentRowIndex++;
            }

            for (int i = currentRowIndex; i < rows.Count; i++)
            {
                WordUtils.AddCellToRow(rows[i], "", noteCellProperties, _valueParagraphProperties);
            }
        }
    }
}
