using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class StudentListPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public StudentListPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.StudentList);
            if (pages == null) throw new ArgumentException(nameof(pages));

            foreach (var page in pages)
            {
                AppendTitle();

                AppendTable(page.StudentList);

                WordUtils.AppendPageBreak(_documentBody);
            }
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines() { After ="0" }),
                new Run(
                    WordUtils.GetRunProperties(bold:true),
                    new Text("СПИСОК СТУДЕНТОВ"))
            );

            _documentBody.Append(title);
        }

        private void AppendTable(List<StudentListRecord> list)
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
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
            );

            //TableCellMargin cellMargin = new TableCellMargin(
            //    new TopMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
            //    new BottomMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
            //    new LeftMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
            //    new RightMargin { Width = "100", Type = TableWidthUnitValues.Dxa }
            //);

            //cellProperties.Append(cellMargin);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = "650" },
                new GridColumn() { Width = "6050" },
                new GridColumn() { Width = "1650" },
                new GridColumn() { Width = "1650" }
            );
            table.AppendChild(tableGrid);

            TableRow headRow = new TableRow();

            TableCell numberHeadCell = new TableCell(new Paragraph(
                paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("№"))));
            var numberHeadCellProperties = cellProperties.CloneNode(true);
            numberHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "650" });
            numberHeadCell.Append(numberHeadCellProperties);

            TableCell studentHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Фамилия, имя отчество (полностью)"))));
            var studentHeadCellProperties = cellProperties.CloneNode(true);
            studentHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "6050" });
            studentHeadCell.Append(studentHeadCellProperties);

            TableCell phoneHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("Контактный телефон"))));
            var phoneHeadCellProperties = cellProperties.CloneNode(true);
            phoneHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1650" });
            phoneHeadCell.Append(phoneHeadCellProperties);

            TableCell cardNumberHeadCell = new TableCell(
                new Paragraph(paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(bold: true),
                        new Text("№ страни-"),
                        new Break()),
                    new Run(WordUtils.GetRunProperties(bold: true),
                        new Text("цы персо-"),
                        new Break()),
                    new Run(WordUtils.GetRunProperties(bold: true),
                        new Text("нифицы-"),
                        new Break()),
                    new Run(WordUtils.GetRunProperties(bold: true),
                        new Text("рованного"),
                        new Break()),
                    new Run(WordUtils.GetRunProperties(bold: true),
                        new Text("учета"))
                ));
            var cardNumberHeadCellProperties = cellProperties.CloneNode(true);
            cardNumberHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1650" });
            cardNumberHeadCell.Append(cardNumberHeadCellProperties);

            headRow.Append(numberHeadCell, studentHeadCell, phoneHeadCell, cardNumberHeadCell);
            table.Append(headRow);

            foreach (var record in list)
            {
                TableRow row = new TableRow();

                TableCell numberCell = new TableCell(new Paragraph(
                    paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Number == null ? "" : ((int)record.Number).ToString()))));
                numberCell.Append(numberHeadCellProperties.CloneNode(true));

                TableCell studentCell = new TableCell(new Paragraph(new ParagraphProperties(new SpacingBetweenLines { After = "0", Before = "0" }),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.Student == null ? "" : GetStudentFIO(record.Student)))));
                studentCell.Append(studentHeadCellProperties.CloneNode(true));

                TableCell phoneCell = new TableCell(new Paragraph(new ParagraphProperties(new SpacingBetweenLines { After = "0", Before = "0" }),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text((record.Student == null || record.Student.PhoneNumber == null) ? "" : record.Student.PhoneNumber))));
                phoneCell.Append(phoneHeadCellProperties.CloneNode(true));

                TableCell cardNumberCell = new TableCell(new Paragraph(new ParagraphProperties(new SpacingBetweenLines { After = "0", Before = "0" }),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.PersonalizedAccountingCardId == null ? "" : record.PersonalizedAccountingCardId.ToString()))));
                cardNumberCell.Append(cardNumberHeadCellProperties.CloneNode(true));

                row.Append(numberCell, studentCell, phoneCell, cardNumberCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }

        private string GetStudentFIO(Student student)
        {
            return student.LastName + " " + student.FirstName + " " + student.MiddleName;
        }
    }
}
