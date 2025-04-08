using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Application.Services.Word
{
    public class IndividualWorkWithStudentGenerator
    {
        private readonly List<IndividualWorkWithStudentRecord> _individualWorkWithStudent;

        private readonly Body _documentBody;

        public IndividualWorkWithStudentGenerator(List<IndividualWorkWithStudentRecord> individualWorkWithStudent, Body body)
        {
            _individualWorkWithStudent = individualWorkWithStudent;
            _documentBody = body;
        }

        public void Generate()
        {
            AppendTitle();
            AppendTable();
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("ИНДИВИДУАЛЬНАЯ РАБОТА СО СТУДЕНТОМ"))
            );

            _documentBody.Append(title);
        }

        private void AppendTable()
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
                new GridColumn() { Width = "950" },
                new GridColumn() { Width = "7500" },
                new GridColumn() { Width = "1550" }
            );
            table.AppendChild(tableGrid);

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Дата"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "950" }));

            TableCell workDoneHeadCell = new TableCell(new Paragraph(
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Проведенная работа и рекомендации ППС,"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("специалистов СППС"))
            ));
            workDoneHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "7500" }));

            TableCell resultHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(bold: true),
                new Text("Результат"))));
            resultHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1550" }));

            headRow.Append(dateHeadCell, workDoneHeadCell, resultHeadCell);
            table.Append(headRow);

            foreach (var record in _individualWorkWithStudent)
            {
                TableRow row = new TableRow();

                TableCell dateCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "950" }));

                TableCell workDoneCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.WorkDoneAndRecommendations == null ? "" : record.WorkDoneAndRecommendations))));
                workDoneCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "7500" }));

                TableCell resultCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Result == null ? "" : record.Result))));
                resultCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1550" }));

                row.Append(dateCell, workDoneCell, resultCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }
    }
}
