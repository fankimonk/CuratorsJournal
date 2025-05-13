using Application.Utils;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Application.Services.Word
{
    public class IndividualWorkWithStudentGenerator
    {
        private readonly List<IndividualWorkWithStudentRecord> _individualWorkWithStudent;

        private readonly Body _documentBody;

        private readonly TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
        );

        private readonly ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        private readonly UInt32Value _valueRowHeight = 590;

        private readonly int _maxRows = 15;

        private readonly ParagraphProperties _paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

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
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines() { After = "50", Before = "0" }),
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

            TableCellProperties cellProperties = new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Top }
            );

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = "950" },
                new GridColumn() { Width = "7500" },
                new GridColumn() { Width = "1550" }
            );
            table.AppendChild(tableGrid);

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата"))));
            var dateHeadCellProperties = cellProperties.CloneNode(true);
            dateHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "950" });
            dateHeadCell.Append(dateHeadCellProperties);

            TableCell workDoneHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Проведенная работа и рекомендации ППС,"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("специалистов СППС"))
            ));
            var workDoneHeadCellProperties = cellProperties.CloneNode(true);
            workDoneHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "7500" });
            workDoneHeadCell.Append(workDoneHeadCellProperties);

            TableCell resultHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Результат"))));
            var resultHeadCellProperties = cellProperties.CloneNode(true);
            resultHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1550" });
            resultHeadCell.Append(resultHeadCellProperties);

            headRow.Append(dateHeadCell, workDoneHeadCell, resultHeadCell);
            table.Append(headRow);

            int rowCount = 0;
            foreach (var record in _individualWorkWithStudent)
            {
                if (rowCount + 1 > _maxRows) break;
                rowCount++;

                TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

                TableCell dateCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                var dateCellProperties = _cellProperties.CloneNode(true);
                dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "950" });
                dateCell.Append(dateCellProperties);

                TableCell workDoneCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.WorkDoneAndRecommendations == null ? "" : record.WorkDoneAndRecommendations))));
                var workDoneCellProperties = _cellProperties.CloneNode(true);
                workDoneCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "7500" });
                workDoneCell.Append(workDoneCellProperties);

                TableCell resultCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true), 
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Result == null ? "" : record.Result))));
                var resultCellProperties = _cellProperties.CloneNode(true);
                resultCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1550" });
                resultCell.Append(resultCellProperties);

                row.Append(dateCell, workDoneCell, resultCell);
                table.Append(row);
            }

            for (int i = rowCount; i < _maxRows; i++) AppendEmptyRow(table);

            _documentBody.Append(table);
        }

        private void AppendEmptyRow(Table table)
        {
            TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

            TableCell dateCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            var dateCellProperties = _cellProperties.CloneNode(true);
            dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "950" });
            dateCell.Append(dateCellProperties);

            TableCell workDoneCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            var workDoneCellProperties = _cellProperties.CloneNode(true);
            workDoneCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "7500" });
            workDoneCell.Append(workDoneCellProperties);

            TableCell resultCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            var resultCellProperties = _cellProperties.CloneNode(true);
            resultCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1550" });
            resultCell.Append(resultCellProperties);

            row.Append(dateCell, workDoneCell, resultCell);
            table.Append(row);
        }
    }
}
