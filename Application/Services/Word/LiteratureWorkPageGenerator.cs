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

            TableRow headRow = new TableRow();

            TableCell dataHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Автор, название, библиографические данные"))));
            dataHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dataColumnWidth.ToString() }));

            TableCell annotationHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("Краткая аннотация"))));
            annotationHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = annotationColumnWidth.ToString() }));

            headRow.Append(dataHeadCell, annotationHeadCell);
            table.Append(headRow);

            foreach (var record in literatureWork)
            {
                TableRow row = new TableRow();

                TableCell dataCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Literature == null ? "" : GetLiteratureStr(record.Literature)))));
                dataCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dataColumnWidth.ToString() }));

                TableCell annotationCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.ShortAnnotation == null ? "" : record.ShortAnnotation))));
                annotationCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = annotationColumnWidth.ToString() }));

                row.Append(dataCell, annotationCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }

        private string GetLiteratureStr(LiteratureListRecord literature)
        {
            return literature.Author + ", \"" + literature.Name + "\", " + literature.BibliographicData;
        }
    }
}
