using Application.Utils;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Application.Services.Word
{
    public class StudentEncouragementsGenerator
    {
        private readonly List<StudentEcouragement> _studentEcouragements;

        private readonly Body _documentBody;

        public StudentEncouragementsGenerator(List<StudentEcouragement> studentEcouragements, Body body)
        {
            _studentEcouragements = studentEcouragements;
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
                    new Justification { Val = JustificationValues.Start }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Поощрения студента")));

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
                new GridColumn() { Width = "1250" },
                new GridColumn() { Width = "5350" },
                new GridColumn() { Width = "3400" }
            );
            table.AppendChild(tableGrid);

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(),
                    new Text("Дата"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1250" }));

            TableCell achievementHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(),
                new Text("За какие достижения"))));
            achievementHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "5350" }));

            TableCell encouragementKindHeadCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(),
                new Text("Вид поощрения"))));
            encouragementKindHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "3400" }));

            headRow.Append(dateHeadCell, achievementHeadCell, encouragementKindHeadCell);
            table.Append(headRow);

            foreach (var record in _studentEcouragements)
            {
                TableRow row = new TableRow();

                TableCell dateCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1250" }));

                TableCell achievementCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.Achievement == null ? "" : record.Achievement))));
                achievementCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "5350" }));

                TableCell encouragementKindCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(record.EncouragementKind == null ? "" : record.EncouragementKind))));
                encouragementKindCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "3400" }));

                row.Append(dateCell, achievementCell, encouragementKindCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }
    }
}
