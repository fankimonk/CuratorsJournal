using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.Literature;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class LiteratureWorkPageGenerator
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

        private readonly int _dataColumnWidth = 2000 * 3;
        private readonly int _annotationColumnWidth = 3000 * 3;

        public LiteratureWorkPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.LiteratureWork);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                var rows = GenerateRows(page.LiteratureWork);
                AppendTable(rows);
            }
            else
            {
                foreach (var p in pages)
                {
                    var rows = GenerateRows(p.LiteratureWork);
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
                    new Text("РАБОТА С НАУЧНО-МЕТОДИЧЕСКОЙ И ПЕДАГОГИЧЕСКОЙ ЛИТЕРАТУРОЙ"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("по вопросам идеологии и воспитания"))
            );

            _documentBody.Append(title);
        }

        private List<TableRow> GenerateRows(List<LiteratureWorkRecord> literatureWork)
        {
            var totalRows = new List<TableRow>();
            foreach (var record in literatureWork)
            {
                var rows = new List<TableRow>() { new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })) };

                var dataStr = record.Literature == null ? "" : GetLiteratureStr(record.Literature);
                var dataCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                dataCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dataColumnWidth.ToString() });
                AppendLiterature(dataStr, rows, dataCellProperties);

                var annotationStr = record.ShortAnnotation ?? "";
                var annotaionCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                annotaionCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _annotationColumnWidth.ToString() });
                AppendAnnotation(annotationStr, rows, annotaionCellProperties, dataCellProperties);

                //foreach (var row in rows) table.Append(row);
                totalRows.AddRange(rows);
            }

            //_documentBody.Append(table);
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
                new GridColumn() { Width = _dataColumnWidth.ToString() },
                new GridColumn() { Width = _annotationColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableRow headRow = new TableRow();

            TableCell dataHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Автор, название, библиографические данные"))));
            dataHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dataColumnWidth.ToString() }));

            TableCell annotationHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Краткая аннотация"))));
            annotationHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _annotationColumnWidth.ToString() }));

            headRow.Append(dataHeadCell, annotationHeadCell);
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

            TableCell dataCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            dataCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dataColumnWidth.ToString() }));

            TableCell annotationCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            annotationCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _annotationColumnWidth.ToString() }));

            row.Append(dataCell, annotationCell);
            table.Append(row);
        }

        private string GetLiteratureStr(LiteratureListRecord literature)
        {
            return literature.Author + ", \"" + literature.Name + "\", " + literature.BibliographicData;
        }

        private void AppendLiterature(string str, List<TableRow> rows, TableCellProperties cellProperties)
        {
            var split = str.Split(' ');
            var lines = new List<string>();
            string currentLine = "";
            int lineMaxChars = 50;
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

        private void AppendAnnotation(string str, List<TableRow> rows,
            TableCellProperties annotationCellProperties, TableCellProperties literatureCellProperties)
        {
            var split = str.Split(' ');
            var lines = new List<string>();
            string currentLine = "";
            int lineMaxChars = 75;
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
                        WordUtils.AddCellToRow(rows[currentRowIndex], "", literatureCellProperties, _valueParagraphProperties, "24");
                    }
                    WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, annotationCellProperties, _valueParagraphProperties, "24");
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
                    WordUtils.AddCellToRow(rows[currentRowIndex], "", literatureCellProperties, _valueParagraphProperties, "24");
                }
                WordUtils.AddCellToRow(rows[currentRowIndex], currentLine, annotationCellProperties, _valueParagraphProperties, "24");
                currentRowIndex++;
            }

            for (int i = currentRowIndex; i < rows.Count; i++)
            {
                WordUtils.AddCellToRow(rows[i], "", annotationCellProperties, _valueParagraphProperties, "24");
            }
        }
    }
}
