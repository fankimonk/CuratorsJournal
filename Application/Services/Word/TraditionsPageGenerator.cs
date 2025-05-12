using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;
using Frontend.Utils;

namespace Application.Services.Word
{
    public class TraditionsPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private readonly TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
        );

        private readonly ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        private readonly UInt32Value _valueRowHeight = 530;

        private readonly int _maxRows = 15;

        private readonly int _nameColumnWidth = 1250 * 3;
        private readonly int _dateColumnWidth = 1250 * 3;
        private readonly int _participationFormColumnWidth = 1250 * 3;
        private readonly int _noteColumnWidth = 1250 * 3;

        public TraditionsPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.Traditions);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                var rows = GenerateRows(page.Traditions);
                AppendTable(rows);
            }
            else
            {
                foreach (var p in pages)
                {
                    var rows = GenerateRows(p.Traditions);
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
                    new Text("ТРАДИЦИИ ВУЗА, ФАКУЛЬТЕТА, ГРУППЫ"))
            );

            _documentBody.Append(title);
        }

        private List<TableRow> GenerateRows(List<Tradition> traditions)
        {
            var totalRows = new List<TableRow>();
            foreach (var tradition in traditions)
            {
                var rows = new List<TableRow>() { new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })) };

                var nameStr = tradition.Name ?? "";
                var nameCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                nameCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _nameColumnWidth.ToString() });
                AppendName(nameStr, rows, nameCellProperties);

                string dateStr = "";
                if (tradition.Day != null && tradition.Month != null) dateStr = tradition.Day + " " + MonthsUtils.MonthsDateNames[(int)tradition.Month];

                var dateCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() });
                TableCell dateCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(dateStr))));
                dateCell.Append(dateCellProperties);
                rows[0].Append(dateCell);
                for (int i = 1; i < rows.Count; i++) WordUtils.AddCellToRow(rows[i], "", nameCellProperties, _valueParagraphProperties, "24");

                var participationFormStr = tradition.ParticipationForm ?? "";
                var participationFormCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                participationFormCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _participationFormColumnWidth.ToString() });
                AppendParticipationForm(participationFormStr, rows, participationFormCellProperties, nameCellProperties, dateCellProperties);

                var noteStr = tradition.Note ?? "";
                var noteCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                noteCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _noteColumnWidth.ToString() });
                AppendNote(noteStr, rows, noteCellProperties, participationFormCellProperties, nameCellProperties, dateCellProperties);

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
                new GridColumn() { Width = _nameColumnWidth.ToString() },
                new GridColumn() { Width = _dateColumnWidth.ToString() },
                new GridColumn() { Width = _participationFormColumnWidth.ToString() },
                new GridColumn() { Width = _noteColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableRow headRow = new TableRow();

            TableCell nameHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Название традиции"))));
            nameHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _nameColumnWidth.ToString() }));

            TableCell dateHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() }));

            TableCell participationFormHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Форма участия группы"))));
            participationFormHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _participationFormColumnWidth.ToString() }));

            TableCell noteHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Примечание"))));
            noteHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _noteColumnWidth.ToString() }));

            headRow.Append(nameHeadCell, dateHeadCell, participationFormHeadCell, noteHeadCell);
            table.Append(headRow);

            int pageCount = rows.Count / _maxRows;
            pageCount += rows.Count % _maxRows == 0 ? 0 : 1;
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

        private void AppendEmptyRow(Table table)
        {
            var row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

            TableCell nameHeadCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            nameHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _nameColumnWidth.ToString() }));
            row.Append(nameHeadCell);

            TableCell dateHeadCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() }));
            row.Append(dateHeadCell);

            TableCell participationFormHeadCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            participationFormHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _participationFormColumnWidth.ToString() }));
            row.Append(participationFormHeadCell);

            TableCell noteHeadCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            noteHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _noteColumnWidth.ToString() }));
            row.Append(noteHeadCell);

            table.Append(row);
        }

        private void AppendName(string str, List<TableRow> rows, TableCellProperties cellProperties)
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
                    }
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, cellProperties, _valueParagraphProperties, "24");
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
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, cellProperties, _valueParagraphProperties, "24");
                currentRowIndex++;
            }
        }

        private void AppendParticipationForm(string str, List<TableRow> rows, TableCellProperties participationFormCellProperties,
            TableCellProperties nameCellProperties, TableCellProperties dateCellProperties)
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", nameCellProperties, _valueParagraphProperties, "24");
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                    }
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, participationFormCellProperties, _valueParagraphProperties, "24");
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
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", nameCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, participationFormCellProperties, _valueParagraphProperties, "24");
                currentRowIndex++;
            }

            for (int i = currentRowIndex; i < rows.Count; i++)
            {
                WordUtils.AddCellToRow(rows[i], "", participationFormCellProperties, _valueParagraphProperties, "24");
            }
        }

        private void AppendNote(string str, List<TableRow> rows, TableCellProperties noteCellProperties, TableCellProperties participationFormCellProperties,
            TableCellProperties nameCellProperties, TableCellProperties dateCellProperties)
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", nameCellProperties, _valueParagraphProperties, "24");
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", participationFormCellProperties, _valueParagraphProperties, "24");
                    }
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, noteCellProperties, _valueParagraphProperties, "24");
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
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", nameCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", participationFormCellProperties, _valueParagraphProperties, "24");
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, noteCellProperties, _valueParagraphProperties, "24");
                currentRowIndex++;
            }

            for (int i = currentRowIndex; i < rows.Count; i++)
            {
                WordUtils.AddCellToRow(rows[i], "", noteCellProperties, _valueParagraphProperties, "24");
            }
        }
    }
}
