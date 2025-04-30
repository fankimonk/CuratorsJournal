using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class InformationHoursAccountingPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
        );

        private ParagraphProperties _paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
            new SpacingBetweenLines { Before = "0", After = "0" });
        private ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        public InformationHoursAccountingPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.InformationHoursAccounting);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                AppendTitle();

                AppendTable(page.InformationHoursAccounting);

                WordUtils.AppendPageBreak(_documentBody);
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

        private void AppendTable(List<InformationHoursAccountingRecord> records)
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

            int dateColumnWidth = 875 * 3;
            int topicColumnWidth = 1625 * 3;
            int noteColumnWidth = 2500 * 3;

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = dateColumnWidth.ToString() },
                new GridColumn() { Width = topicColumnWidth.ToString() },
                new GridColumn() { Width = noteColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата проведения"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() }));

            TableCell topicHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Тема"))));
            topicHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = topicColumnWidth.ToString() }));

            TableCell noteHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Примечание"))));
            noteHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() }));

            headRow.Append(dateHeadCell, topicHeadCell, noteHeadCell);
            table.Append(headRow);

            foreach (var record in records)
            {
                var rows = new List<TableRow>() { new TableRow() };

                var dateCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() });

                TableCell dateCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(dateCellProperties);
                rows[0].Append(dateCell);

                var topicStr = record.Topic ?? "";
                var topicCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                topicCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = topicColumnWidth.ToString() });

                AppendTopic(topicStr, rows, topicCellProperties, dateCellProperties, table);

                var noteStr = record.Note ?? "";
                var noteCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                noteCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() });

                AppendNote(noteStr, rows, noteCellProperties, topicCellProperties, dateCellProperties, table);

                foreach (var row in rows) table.Append(row);
            }

            _documentBody.Append(table);
        }

        private void AppendTopic(string topicStr, List<TableRow> rows, TableCellProperties topicCellProperties, 
            TableCellProperties dateCellProperties, Table table)
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
                        rows.Add(new TableRow());
                        AddCellToRow(rows[currentTopicRowIndex], "", dateCellProperties, _valueParagraphProperties);
                    }
                    AddCellToRow(rows[currentTopicRowIndex], currentTopicLine, topicCellProperties, _valueParagraphProperties);
                    topicLines.Add(currentTopicLine);
                    currentTopicRowIndex++;
                    currentTopicLine = word + " ";
                }
            }

            if (!string.IsNullOrEmpty(currentTopicLine) && !topicLines.Contains(currentTopicLine))
            {
                if (currentTopicRowIndex == rows.Count)
                {
                    rows.Add(new TableRow());
                    AddCellToRow(rows[currentTopicRowIndex], "", dateCellProperties, _valueParagraphProperties);
                }
                AddCellToRow(rows[currentTopicRowIndex], currentTopicLine, topicCellProperties, _valueParagraphProperties);
                currentTopicRowIndex++;
            }

            for (int i = currentTopicRowIndex; i < rows.Count; i++)
            {
                AddCellToRow(rows[i], "", topicCellProperties, _valueParagraphProperties);
            }
        }

        private void AppendNote(string noteStr, List<TableRow> rows, TableCellProperties noteCellProperties,
            TableCellProperties topicCellProperties, TableCellProperties dateCellProperties, Table table)
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
                        rows.Add(new TableRow());
                        AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties);
                        AddCellToRow(rows[currentRowIndex], "", topicCellProperties, _valueParagraphProperties);
                    }
                    AddCellToRow(rows[currentRowIndex], currentLine, noteCellProperties, _valueParagraphProperties);
                    lines.Add(currentLine);
                    currentRowIndex++;
                    currentLine = word + " ";
                }
            }

            if (!string.IsNullOrEmpty(currentLine) && !lines.Contains(currentLine))
            {
                if (currentRowIndex == rows.Count)
                {
                    rows.Add(new TableRow());
                    AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties);
                    AddCellToRow(rows[currentRowIndex], "", topicCellProperties, _valueParagraphProperties);
                }
                AddCellToRow(rows[currentRowIndex], currentLine, noteCellProperties, _valueParagraphProperties);
                currentRowIndex++;
            }

            for (int i = currentRowIndex; i < rows.Count; i++)
            {
                AddCellToRow(rows[i], "", noteCellProperties, _valueParagraphProperties);
            }
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
