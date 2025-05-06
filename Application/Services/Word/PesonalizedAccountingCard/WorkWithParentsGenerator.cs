using Application.Utils;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Application.Services.Word
{
    public class WorkWithParentsGenerator
    {
        private readonly List<WorkWithParentsRecord> _workWithParents;

        private readonly Body _documentBody;

        private ParagraphProperties _valueParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" });

        private UInt32Value _valueRowHeight = 480;

        public WorkWithParentsGenerator(List<WorkWithParentsRecord> workWithParents, Body body)
        {
            _workWithParents = workWithParents;
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
                    new Text("РАБОТА С РОДИТЕЛЯМИ/РОДСТВЕННИКАМИ,"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("ЛИЦАМИ, ИХ ЗАМЕНЯЮЩИМИ"))
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

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = "1000" },
                new GridColumn() { Width = "5600" },
                new GridColumn() { Width = "3400" }
            );
            table.AppendChild(tableGrid);

            TableRow headRow = new TableRow();

            TableCell dateHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Дата"))));
            dateHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1000" }));

            TableCell workContentHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Содержание работы"))
            ));
            workContentHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "5600" }));

            TableCell noteHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Примечание"))));
            noteHeadCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "3400" }));

            headRow.Append(dateHeadCell, workContentHeadCell, noteHeadCell);
            table.Append(headRow);

            foreach (var record in _workWithParents)
            {
                TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

                TableCell dateCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Date == null ? "" : ((DateOnly)record.Date).ToString()))));
                dateCell.Append(new TableCellProperties(
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1000" }));

                TableCell workContentCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true), 
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.WorkContent == null ? "" : record.WorkContent))));
                workContentCell.Append(new TableCellProperties(
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "5600" }));

                TableCell noteCell = new TableCell(new Paragraph(_valueParagraphProperties.CloneNode(true), 
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Note == null ? "" : record.Note))));
                noteCell.Append(new TableCellProperties(
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "3400" }));

                row.Append(dateCell, workContentCell, noteCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }
    }
}
