using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class RecommendationsAndRemarksPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public RecommendationsAndRemarksPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.RecomendationsAndRemarks);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                AppendTitle();

                AppendTable(page.RecomendationsAndRemarks);

                WordUtils.AppendPageBreak(_documentBody);
            }
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("РЕКОМЕНДАЦИИ И ЗАМЕЧАНИЯ ЛИЦ, ПРОВЕРЯЮЩИХ РАБОТУ КУРАТОРА"))
            );

            _documentBody.Append(title);
        }

        private void AppendTable(List<RecomendationsAndRemarksRecord> records)
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

            int dateColumnWidth = 375 * 3;
            int reviewerColumnWidth = 1280 * 3;
            int contentColumnWidth = 2490 * 3;
            int resultColumnWidth = 855 * 3;

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = dateColumnWidth.ToString() },
                new GridColumn() { Width = reviewerColumnWidth.ToString() },
                new GridColumn() { Width = contentColumnWidth.ToString() },
                new GridColumn() { Width = resultColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Дата"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() }));

            TableCell reviewerHeadCell = new TableCell(new Paragraph(
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Кто проверил"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("(должность, фамилия)"))));
            reviewerHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = reviewerColumnWidth.ToString() }));

            TableCell contentHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("Рекомендации, замечания"))));
            contentHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = contentColumnWidth.ToString() }));

            TableCell resultHeadCell = new TableCell(new Paragraph(
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Выполнение,"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Дата"))));
            resultHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = resultColumnWidth.ToString() }));

            headRow.Append(dateHeadCell, reviewerHeadCell, contentHeadCell, resultHeadCell);
            table.Append(headRow);

            foreach (var record in records)
            {
                TableRow row = new TableRow();

                TableCell dateCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = dateColumnWidth.ToString() }));

                TableCell reviewerCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Reviewer == null ? "" : GetReviewerString(record.Reviewer)))));
                reviewerCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = reviewerColumnWidth.ToString() }));

                TableCell contentCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Content == null ? "" : record.Content))));
                contentCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = contentColumnWidth.ToString() }));

                string resultStr = "";
                if (record.Result != null) resultStr += record.Result + ", ";
                if (record.ExecutionDate != null) resultStr += record.ExecutionDate.ToString();

                TableCell resultCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(resultStr))));
                resultCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = resultColumnWidth.ToString() }));

                row.Append(dateCell, reviewerCell, contentCell, resultCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }

        private string GetReviewerString(Worker worker)
        {
            string positionStr = "";
            if (worker.Position != null) positionStr += worker.Position.Name + ", ";
            return positionStr + worker.LastName + " " + worker.FirstName[0] + ". " + worker.MiddleName[0] + ".";
        }
    }
}
