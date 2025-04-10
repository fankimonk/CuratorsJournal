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

        public TraditionsPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.Traditions);
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

            TableRow headRow = new TableRow();

            TableCell nameHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Название традиции"))));
            nameHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = nameColumnWidth.ToString() }));

            TableCell dateHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("Дата"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() }));

            TableCell participationFormHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("Форма участия группы"))));
            participationFormHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = participationFormColumnWidth.ToString() }));

            TableCell noteHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Примечание"))));
            noteHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() }));

            headRow.Append(nameHeadCell, dateHeadCell, participationFormHeadCell, noteHeadCell);
            table.Append(headRow);

            foreach (var tradition in traditions)
            {
                TableRow row = new TableRow();

                TableCell nameCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(tradition.Name == null ? "" : tradition.Name))));
                nameCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = nameColumnWidth.ToString() }));

                string dateStr = "";
                if (tradition.Day != null && tradition.Month != null) dateStr = tradition.Day + MonthsUtils.MonthsDateNames[(int)tradition.Month];

                TableCell dateCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(dateStr))));
                dateCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() }));

                TableCell participationFormCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(tradition.ParticipationForm == null ? "" : tradition.ParticipationForm))));
                participationFormCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = participationFormColumnWidth.ToString() }));

                TableCell noteCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(tradition.Note == null ? "" : tradition.Note))));
                noteCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() }));

                row.Append(nameCell, dateCell, participationFormCell, noteCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }
    }
}
