using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class CuratorsParticipationPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public CuratorsParticipationPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.CuratorsParticipationInPedagogicalSeminars);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                AppendTitle();

                AppendTable(page.CuratorsParticipationInPedagogicalSeminars);

                WordUtils.AppendPageBreak(_documentBody);
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

        private void AppendTable(List<CuratorsParticipationInPedagogicalSeminarsRecord> records)
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

            int dateColumnWidth = 400 * 3;
            int topicColumnWidth = 1300 * 3;
            int participationFormColumnWidth = 700 * 3;
            int locationColumnWidth = 800 * 3;
            int noteColumnWidth = 1800 * 3;

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = dateColumnWidth.ToString() },
                new GridColumn() { Width = topicColumnWidth.ToString() },
                new GridColumn() { Width = participationFormColumnWidth.ToString() },
                new GridColumn() { Width = locationColumnWidth.ToString() },
                new GridColumn() { Width = noteColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() }));

            TableCell topicHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Тема семинара заседания"))));
            topicHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = topicColumnWidth.ToString() }));

            TableCell participationFormHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Форма участия"))));
            participationFormHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = participationFormColumnWidth.ToString() }));

            TableCell locationHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Место проведения"))));
            locationHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = locationColumnWidth.ToString() }));

            TableCell noteHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Примечание"))));
            noteHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() }));

            headRow.Append(dateHeadCell, topicHeadCell, participationFormHeadCell, locationHeadCell, noteHeadCell);
            table.Append(headRow);

            foreach (var record in records)
            {
                TableRow row = new TableRow();

                TableCell dateCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() }));

                TableCell topicCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Topic ?? ""))));
                topicCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = topicColumnWidth.ToString() }));

                TableCell participationFormCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.ParticipationForm ?? ""))));
                participationFormCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = participationFormColumnWidth.ToString() }));

                TableCell locationCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.SeminarLocation ?? ""))));
                locationCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = locationColumnWidth.ToString() }));

                TableCell noteCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Note ?? ""))));
                noteCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() }));

                row.Append(dateCell, topicCell, participationFormCell, locationCell, noteCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }
    }
}
