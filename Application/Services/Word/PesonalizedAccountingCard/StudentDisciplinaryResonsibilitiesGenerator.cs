using Application.Utils;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Application.Services.Word
{
    public class StudentDisciplinaryResonsibilitiesGenerator
    {
        private readonly List<StudentDisciplinaryResponsibility> _studentDisciplinaryResponsibilities;

        private readonly Body _documentBody;

        private ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        private readonly int _maxRows = 3;

        public StudentDisciplinaryResonsibilitiesGenerator(List<StudentDisciplinaryResponsibility> studentDisciplinaryResponsibilities, Body body)
        {
            _studentDisciplinaryResponsibilities = studentDisciplinaryResponsibilities;
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
                    new Justification { Val = JustificationValues.Start },
                    new SpacingBetweenLines() { After = "0", Before = "50" }),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Отметки о дисциплинарной ответственности")));

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

            ParagraphProperties paragraphProperties = new ParagraphProperties(
                new SpacingBetweenLines { Before = "0", After = "0" });

            ParagraphProperties headParagraphProperties = (ParagraphProperties)paragraphProperties.CloneNode(true);
            Tabs tabs = new Tabs();
            tabs.Append(new TabStop()
            {
                Val = TabStopValues.Left,
                Position = 360
            });
            headParagraphProperties.Append(tabs);

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(headParagraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new TabChar(),
                    new Text("Дата"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1250" }));

            TableCell achievementHeadCell = new TableCell(new Paragraph(headParagraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new TabChar(),
                    new Text("За какой проступок"))));
            achievementHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "5350" }));

            TableCell encouragementKindHeadCell = new TableCell(new Paragraph(headParagraphProperties.CloneNode(true), 
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new TabChar(),
                    new Text("Вид"))));
            encouragementKindHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "3400" }));

            headRow.Append(dateHeadCell, achievementHeadCell, encouragementKindHeadCell);
            table.Append(headRow);

            int rowCount = 0;
            foreach (var record in _studentDisciplinaryResponsibilities)
            {
                if (rowCount + 1 > _maxRows) break;
                rowCount++;

                TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = 340 }));

                TableCell dateCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true), 
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1250" }));

                TableCell misdemeanorCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true), 
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Misdemeanor == null ? "" : record.Misdemeanor))));
                misdemeanorCell.Append(new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "5350" }));

                TableCell encouragementKindCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true), 
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.DisciplinaryResponsibilityKind == null ? "" : record.DisciplinaryResponsibilityKind))));
                encouragementKindCell.Append(new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "3400" }));

                row.Append(dateCell, misdemeanorCell, encouragementKindCell);
                table.Append(row);
            }

            for (int i = rowCount; i < _maxRows; i++) AppendEmptyRow(table);

            _documentBody.Append(table);
        }

        private void AppendEmptyRow(Table table)
        {
            TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = 340 }));

            TableCell dateCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            dateCell.Append(new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1250" }));

            TableCell misdemeanorCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            misdemeanorCell.Append(new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "5350" }));

            TableCell encouragementKindCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            encouragementKindCell.Append(new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "3400" }));

            row.Append(dateCell, misdemeanorCell, encouragementKindCell);
            table.Append(row);
        }
    }
}
