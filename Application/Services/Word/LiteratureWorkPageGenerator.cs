using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.Literature;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class LiteratureWorkPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
        );

        private ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        private UInt32Value _valueRowHeight = 420;

        public LiteratureWorkPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.LiteratureWork);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                AppendTitle();
                AppendTable(page.LiteratureWork);
                if (page != pages.Last()) WordUtils.AppendPageBreak(_documentBody);
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

        private void AppendTable(List<LiteratureWorkRecord> literatureWork)
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

            int dataColumnWidth = 2000 * 3;
            int annotationColumnWidth = 3000 * 3;

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = dataColumnWidth.ToString() },
                new GridColumn() { Width = annotationColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableRow headRow = new TableRow();

            TableCell dataHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Автор, название, библиографические данные"))));
            dataHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dataColumnWidth.ToString() }));

            TableCell annotationHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Краткая аннотация"))));
            annotationHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = annotationColumnWidth.ToString() }));

            headRow.Append(dataHeadCell, annotationHeadCell);
            table.Append(headRow);

            foreach (var record in literatureWork)
            {
                var rows = new List<TableRow>() { new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight })) };

                var dataStr = record.Literature == null ? "" : GetLiteratureStr(record.Literature);
                var dataCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                dataCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dataColumnWidth.ToString() });
                AppendLiterature(dataStr, rows, dataCellProperties);

                var annotationStr = record.ShortAnnotation ?? "";
                var annotaionCellProperties = (TableCellProperties)_cellProperties.CloneNode(true);
                annotaionCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = annotationColumnWidth.ToString() });
                AppendAnnotation(annotationStr, rows, annotaionCellProperties, dataCellProperties);

                foreach (var row in rows) table.Append(row);
            }

            _documentBody.Append(table);
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
