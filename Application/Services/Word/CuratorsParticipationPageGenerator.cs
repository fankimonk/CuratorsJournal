using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class CuratorsParticipationPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
        );

        private ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        private UInt32Value _valueRowHeight = 410;

        private readonly int _maxRows = 20;

        private readonly int _dateColumnWidth = 400 * 3;
        private readonly int _topicColumnWidth = 1300 * 3;
        private readonly int _participationFormColumnWidth = 700 * 3;
        private readonly int _locationColumnWidth = 800 * 3;
        private readonly int _noteColumnWidth = 1800 * 3;

        public CuratorsParticipationPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.CuratorsParticipationInPedagogicalSeminars);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                var rows = GenerateRows(page.CuratorsParticipationInPedagogicalSeminars);
                AppendTable(rows);
            }
            else
            {
                foreach (var p in pages)
                {
                    var rows = GenerateRows(p.CuratorsParticipationInPedagogicalSeminars);
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
                    new Text("УЧАСТИЕ КУРАТОРА В РАБОТЕ ПЕДАГОГИЧЕСКИХ СЕМИНАРОВ,"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("МЕТОДИЧЕСКОГО ОБЪЕДИНЕНИЯ"))
            );

            _documentBody.Append(title);
        }

        private List<TableRow> GenerateRows(List<CuratorsParticipationInPedagogicalSeminarsRecord> records)
        {
            var totalRows = new List<TableRow>();
            foreach (var record in records)
            {
                var rows = new List<TableRow>() { new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })) };

                var dateStr = record.Date == null ? "" : ((DateOnly)record.Date).ToString();
                var dateCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() });
                TableCell dateCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(dateCellProperties);
                rows[0].Append(dateCell);

                var topicStr = record.Topic ?? "";
                var topicCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                topicCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _topicColumnWidth.ToString() });
                AppendTopic(topicStr, rows, topicCellProperties, dateCellProperties);

                var participationFormStr = record.ParticipationForm ?? "";
                var participationFormCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                participationFormCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _participationFormColumnWidth.ToString() });
                AppendParticipationForm(participationFormStr, rows, participationFormCellProperties, topicCellProperties, dateCellProperties);

                var locationStr = record.SeminarLocation ?? "";
                var locationCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                locationCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _locationColumnWidth.ToString() });
                AppendLocation(locationStr, rows, locationCellProperties, participationFormCellProperties, topicCellProperties, dateCellProperties);

                var noteStr = record.Note ?? "";
                var noteCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                noteCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _noteColumnWidth.ToString() });
                AppendNote(noteStr, rows, noteCellProperties, locationCellProperties, participationFormCellProperties, topicCellProperties, dateCellProperties);

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
                new GridColumn() { Width = _topicColumnWidth.ToString() },
                new GridColumn() { Width = _participationFormColumnWidth.ToString() },
                new GridColumn() { Width = _locationColumnWidth.ToString() },
                new GridColumn() { Width = _noteColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() }));

            TableCell topicHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Тема семинара заседания"))));
            topicHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _topicColumnWidth.ToString() }));

            TableCell participationFormHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Форма участия"))));
            participationFormHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _participationFormColumnWidth.ToString() }));

            TableCell locationHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Место проведения"))));
            locationHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _locationColumnWidth.ToString() }));

            TableCell noteHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Примечание"))));
            noteHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _noteColumnWidth.ToString() }));

            headRow.Append(dateHeadCell, topicHeadCell, participationFormHeadCell, locationHeadCell, noteHeadCell);
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

            var dateCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() });
            TableCell dateCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            dateCell.Append(dateCellProperties);

            var topicCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            topicCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _topicColumnWidth.ToString() });
            TableCell topicCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            topicCell.Append(topicCellProperties);

            var participationFormCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            participationFormCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _participationFormColumnWidth.ToString() });
            TableCell participationFormCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            participationFormCell.Append(participationFormCellProperties);

            var locationCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            locationCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _locationColumnWidth.ToString() });
            TableCell locationCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            locationCell.Append(locationCellProperties);

            var noteCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
            noteCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _noteColumnWidth.ToString() });
            TableCell noteCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            noteCell.Append(noteCellProperties);

            row.Append(dateCell, topicCell, participationFormCell, locationCell, noteCell);
            table.Append(row);
        }

        private void AppendTopic(string str, List<TableRow> rows, TableCellProperties topicCellProperties,
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
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, topicCellProperties, _valueParagraphProperties, "24");
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
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, topicCellProperties, _valueParagraphProperties, "24");
                currentRowIndex++;
            }
        }

        private void AppendParticipationForm(string str, List<TableRow> rows, TableCellProperties participationFormCellProperties,
            TableCellProperties topicCellProperties, TableCellProperties dateCellProperties)
        {
            var split = str.Split(' ');
            var lines = new List<string>();
            string currentLine = "";
            int lineMaxChars = 17;
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", topicCellProperties, _valueParagraphProperties, "24");
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
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", topicCellProperties, _valueParagraphProperties, "24");
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, participationFormCellProperties, _valueParagraphProperties, "24");
                currentRowIndex++;
            }

            for (int i = currentRowIndex; i < rows.Count; i++)
            {
                WordUtils.AddCellToRow(rows[i], "", participationFormCellProperties, _valueParagraphProperties, "24");
            }
        }

        private void AppendLocation(string str, List<TableRow> rows, TableCellProperties locationCellProperties, TableCellProperties participationFormCellProperties,
            TableCellProperties topicCellProperties, TableCellProperties dateCellProperties)
        {
            var split = str.Split(' ');
            var lines = new List<string>();
            string currentLine = "";
            int lineMaxChars = 20;
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", topicCellProperties, _valueParagraphProperties, "24");
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", participationFormCellProperties, _valueParagraphProperties, "24");
                    }
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, locationCellProperties, _valueParagraphProperties, "24");
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
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", topicCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", participationFormCellProperties, _valueParagraphProperties, "24");
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, locationCellProperties, _valueParagraphProperties, "24");
                currentRowIndex++;
            }

            for (int i = currentRowIndex; i < rows.Count; i++)
            {
                WordUtils.AddCellToRow(rows[i], "", locationCellProperties, _valueParagraphProperties, "24");
            }
        }

        private void AppendNote(string str, List<TableRow> rows, TableCellProperties noteCellProperties, TableCellProperties locationCellProperties,
            TableCellProperties participationFormCellProperties, TableCellProperties topicCellProperties, TableCellProperties dateCellProperties)
        {
            var split = str.Split(' ');
            var lines = new List<string>();
            string currentLine = "";
            int lineMaxChars = 40;
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", topicCellProperties, _valueParagraphProperties, "24");
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", participationFormCellProperties, _valueParagraphProperties, "24");
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", locationCellProperties, _valueParagraphProperties, "24");
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
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", dateCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", topicCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", participationFormCellProperties, _valueParagraphProperties, "24");
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", locationCellProperties, _valueParagraphProperties, "24");
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
