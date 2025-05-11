using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Frontend.Utils;

namespace Application.Services.Word
{
    public class TraditionsPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
        );

        private ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        private UInt32Value _valueRowHeight = 520;

        private int _maxRowsCount = 15;

        public TraditionsPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.Traditions);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                AppendTitle();

                AppendTable(page.Traditions);

                if (page != pages.Last()) WordUtils.AppendPageBreak(_documentBody);
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

        private void AppendTable(List<Tradition> traditions)
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

            int nameColumnWidth = 1250 * 3;
            int dateColumnWidth = 1250 * 3;
            int participationFormColumnWidth = 1250 * 3;
            int noteColumnWidth = 1250 * 3;

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = nameColumnWidth.ToString() },
                new GridColumn() { Width = dateColumnWidth.ToString() },
                new GridColumn() { Width = participationFormColumnWidth.ToString() },
                new GridColumn() { Width = noteColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableRow headRow = new TableRow();

            TableCell nameHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Название традиции"))));
            nameHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = nameColumnWidth.ToString() }));

            TableCell dateHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() }));

            TableCell participationFormHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Форма участия группы"))));
            participationFormHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = participationFormColumnWidth.ToString() }));

            TableCell noteHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Примечание"))));
            noteHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() }));

            headRow.Append(nameHeadCell, dateHeadCell, participationFormHeadCell, noteHeadCell);
            table.Append(headRow);

            foreach (var tradition in traditions)
            {
                var rows = new List<TableRow>() { new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })) };

                var nameStr = tradition.Name ?? "";
                var nameCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                nameCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = nameColumnWidth.ToString() });
                AppendName(nameStr, rows, nameCellProperties, table);

                string dateStr = "";
                if (tradition.Day != null && tradition.Month != null) dateStr = tradition.Day + " " + MonthsUtils.MonthsDateNames[(int)tradition.Month];

                var dateCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() });
                TableCell dateCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(dateStr))));
                dateCell.Append(dateCellProperties);
                rows[0].Append(dateCell);
                for (int i = 1; i < rows.Count; i++) WordUtils.AddCellToRow(rows[i], "", nameCellProperties, _valueParagraphProperties, "24");

                var participationFormStr = tradition.ParticipationForm ?? "";
                var participationFormCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                participationFormCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = participationFormColumnWidth.ToString() });
                AppendParticipationForm(participationFormStr, rows, participationFormCellProperties, nameCellProperties, dateCellProperties, table);

                var noteStr = tradition.Note ?? "";
                var noteCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                noteCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() });
                AppendNote(noteStr, rows, noteCellProperties, participationFormCellProperties, nameCellProperties, dateCellProperties, table);

                //int rowCount = Math.Min(_maxRowsCount, rows.Count);
                for (int i = 0; i < rows.Count; i++) table.Append(rows[i]);
                //int rowsLeftCount = _maxRowsCount - rowCount;
                //var emptyRow = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));
                //WordUtils.AddCellToRow(emptyRow, "", nameCellProperties, _valueParagraphProperties, "24");
                //WordUtils.AddCellToRow(emptyRow, "", dateCellProperties, _valueParagraphProperties, "24");
                //WordUtils.AddCellToRow(emptyRow, "", participationFormCellProperties, _valueParagraphProperties, "24");
                //WordUtils.AddCellToRow(emptyRow, "", noteCellProperties, _valueParagraphProperties, "24");
                //for (int i = 0; i < rowsLeftCount; i++) table.Append(emptyRow.CloneNode(true));
            }

            _documentBody.Append(table);
        }

        private void AppendName(string str, List<TableRow> rows, TableCellProperties cellProperties, Table table)
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
            TableCellProperties nameCellProperties, TableCellProperties dateCellProperties, Table table)
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
            TableCellProperties nameCellProperties, TableCellProperties dateCellProperties, Table table)
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
