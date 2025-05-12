using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class StudentListPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private int _maxRows = 30;

        private ParagraphProperties _paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

        public StudentListPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.StudentList);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                int pageCount = page.StudentList.Count / _maxRows;
                pageCount += page.StudentList.Count % _maxRows == 0 ? 0 : 1;
                for (int i = 0; i < pageCount; i++)
                {
                    GeneratePage(page.StudentList.Skip(i * _maxRows).Take(_maxRows).ToList());
                }
            }
            else
            {
                foreach (var p in pages)
                {
                    int pageCount = p.StudentList.Count / _maxRows;
                    pageCount += p.StudentList.Count % _maxRows == 0 ? 0 : 1;
                    for (int i = 0; i < pageCount; i++)
                    {
                        GeneratePage(p.StudentList.Skip(i * _maxRows).Take(_maxRows).ToList());
                    }
                }
            }  
        }

        private void GeneratePage(List<StudentListRecord> list)
        {
            AppendTitle();

            AppendTable(list);

            WordUtils.AppendPageBreak(_documentBody);
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines() { After = "0" }),
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
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("№"))));
            var numberHeadCellProperties = cellProperties.CloneNode(true);
            numberHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "650" });
            numberHeadCell.Append(numberHeadCellProperties);

            TableCell studentHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Фамилия, имя отчество (полностью)"))));
            var studentHeadCellProperties = cellProperties.CloneNode(true);
            studentHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "6050" });
            studentHeadCell.Append(studentHeadCellProperties);

            TableCell phoneHeadCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Контактный телефон"))));
            var phoneHeadCellProperties = cellProperties.CloneNode(true);
            phoneHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "1650" });
            phoneHeadCell.Append(phoneHeadCellProperties);

            TableCell cardNumberHeadCell = new TableCell(
                new Paragraph(paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                        new Text("№ страни-"),
                        new Break()),
                    new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                        new Text("цы персо-"),
                        new Break()),
                    new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                        new Text("нифицы-"),
                        new Break()),
                    new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                        new Text("рованного"),
                        new Break()),
                    new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
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
                var rowProperties = new TableRowProperties();
                rowProperties.Append(new TableRowHeight { Val = 400 });
                row.AppendChild(rowProperties);

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

                TableCell cardNumberCell = new TableCell(new Paragraph(paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "24"),
                        new Text(record.PersonalizedAccountingCardId == null ? "" : record.PersonalizedAccountingCardId.ToString()))));
                cardNumberCell.Append(cardNumberHeadCellProperties.CloneNode(true));

                row.Append(numberCell, studentCell, phoneCell, cardNumberCell);
                table.Append(row);
            }

            for (int i = list.Count; i < _maxRows; i++)
                AppendEmptyRow(table, (TableCellProperties)numberHeadCellProperties, (TableCellProperties)studentHeadCellProperties, 
                    (TableCellProperties)phoneHeadCellProperties, (TableCellProperties)cardNumberHeadCellProperties);

            _documentBody.Append(table);
        }

        private string GetStudentFIO(Student student)
        {
            return student.LastName + " " + student.FirstName + " " + student.MiddleName;
        }

        private void AppendEmptyRow(Table table, TableCellProperties numberCellProperties, TableCellProperties studentCellProperties,
            TableCellProperties phoneCellProperties, TableCellProperties cardNumberCellProperties)
        {
            TableRow row = new TableRow();
            var rowProperties = new TableRowProperties();
            rowProperties.Append(new TableRowHeight { Val = 400 });
            row.AppendChild(rowProperties);

            TableCell numberCell = new TableCell(new Paragraph(
                _paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            numberCell.Append(numberCellProperties.CloneNode(true));

            TableCell studentCell = new TableCell(new Paragraph(new ParagraphProperties(new SpacingBetweenLines { After = "0", Before = "0" }),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            studentCell.Append(studentCellProperties.CloneNode(true));

            TableCell phoneCell = new TableCell(new Paragraph(new ParagraphProperties(new SpacingBetweenLines { After = "0", Before = "0" }),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            phoneCell.Append(phoneCellProperties.CloneNode(true));

            TableCell cardNumberCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "24"),
                    new Text(""))));
            cardNumberCell.Append(cardNumberCellProperties.CloneNode(true));

            row.Append(numberCell, studentCell, phoneCell, cardNumberCell);
            table.Append(row);
        }
    }
}
