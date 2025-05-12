using Application.Utils;
using DataAccess.Interfaces;
using DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.FinalPerformanceAccounting;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class FinalPerformanceAccountingPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;
        private readonly IPerformanceAccountingColumnsRepository _columnsRepository;

        private readonly Body _documentBody;

        private readonly int _journalId;

        private List<CertificationType> _certificationTypes = [];

        private readonly UInt32Value _valueRowHeight = 420;

        private readonly int _maxRows = 15;

        private readonly int _numberColumnWidth = 150 * 3;
        private readonly int _studentColumnWidth = 900 * 3;
        private readonly int _subjectColumnWidth = 3950 * 3;

        private readonly ParagraphProperties _paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

        public FinalPerformanceAccountingPageGenerator(int journalId, Body body, IPagesRepository pagesRepository,
            IPerformanceAccountingColumnsRepository columnsRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
            _columnsRepository = columnsRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.FinalPerformanceAccounting);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));

                var certificationTypes = await _columnsRepository.GetByPageIdGroupByCertificationTypes(page.Id);
                if (certificationTypes == null) throw new ArgumentException(nameof(page));
                _certificationTypes = certificationTypes;

                int pageCount = page.FinalPerformanceAccounting.Count / _maxRows;
                pageCount += page.FinalPerformanceAccounting.Count % _maxRows == 0 ? 0 : 1;
                for (int i = 0; i < pageCount; i++)
                {
                    GeneratePage(page.FinalPerformanceAccounting.Skip(i * _maxRows).Take(_maxRows).ToList());
                }
            }
            else
            {
                foreach (var p in pages)
                {
                    var certificationTypes = await _columnsRepository.GetByPageIdGroupByCertificationTypes(p.Id);
                    if (certificationTypes == null) throw new ArgumentException(nameof(p));
                    _certificationTypes = certificationTypes;

                    int pageCount = p.FinalPerformanceAccounting.Count / _maxRows;
                    pageCount += p.FinalPerformanceAccounting.Count % _maxRows == 0 ? 0 : 1;
                    for (int i = 0; i < pageCount; i++)
                    {
                        GeneratePage(p.FinalPerformanceAccounting.Skip(i * _maxRows).Take(_maxRows).ToList());
                    }
                }
            } 
        }

        private void GeneratePage(List<FinalPerformanceAccountingRecord> records)
        {
            AppendTitle();
            AppendTable(records);
            WordUtils.AppendSectionBreak(WordUtils.PageOrientationTypes.Landscape, _documentBody);
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(
                    WordUtils.GetRunProperties(bold:true),
                    new Text("УЧЕТ ИТОГОВОЙ УСПЕВАЕМОСТИ СТУДЕНТОВ ГРУППЫ"))
            );

            _documentBody.Append(title);
        }

        private void AppendTable(List<FinalPerformanceAccountingRecord> records)
        {
            var table = new Table();

            int gradeColumnsCount = GetColumnCount();
            int gradeColumnWidth = _subjectColumnWidth / gradeColumnsCount;

            var tableProperties = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = BorderValues.Single, Size = 4 },
                    new BottomBorder { Val = BorderValues.Single, Size = 4 },
                    new LeftBorder { Val = BorderValues.Single, Size = 4 },
                    new RightBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                )
            );
            table.AppendChild(tableProperties);

            TableCellProperties cellProperties = new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
            );

            var headerRow1 = new TableRow();

            var numberHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"), 
                    new Text("№")))
            );
            var numberHeadCellProperties = cellProperties.CloneNode(true);
            numberHeadCellProperties.Append(
                new VerticalMerge { Val = MergedCellValues.Restart },
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _numberColumnWidth.ToString() });
            numberHeadCell.Append(numberHeadCellProperties);
            headerRow1.AppendChild(numberHeadCell);

            var studentHeadCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"), 
                    new Text("Ф. И. О. студента")))
            );
            var studentHeadCellProperties = cellProperties.CloneNode(true);
            studentHeadCellProperties.Append(
                new VerticalMerge { Val = MergedCellValues.Restart },
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _studentColumnWidth.ToString() });
            studentHeadCell.Append(studentHeadCellProperties);
            headerRow1.AppendChild(studentHeadCell);

            var subjectHeadCell = new TableCell(
                new TableCellProperties(
                    new GridSpan { Val = gradeColumnsCount },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _subjectColumnWidth.ToString() }
                ),
                new Paragraph(_paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"), new Text("Название дисциплины")))
            );
            headerRow1.AppendChild(subjectHeadCell);

            table.AppendChild(headerRow1);

            var headerRow2 = new TableRow();

            var numberEmptyHeadCell1 = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Continue },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _numberColumnWidth.ToString() }
                ),
                new Paragraph(_paragraphProperties.CloneNode(true))
            );
            headerRow2.AppendChild(numberEmptyHeadCell1);

            var studentEmptyHeadCell1 = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Continue },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _studentColumnWidth.ToString() }
                ),
                new Paragraph(_paragraphProperties.CloneNode(true))
            );
            headerRow2.AppendChild(studentEmptyHeadCell1);

            var certificationTypes = GetNotEmptyCertificationTypes();
            foreach (var ct in certificationTypes)
            {
                var certificationTypeHeadCell = new TableCell(
                    new TableCellProperties(
                        new GridSpan { Val = ct.PerformanceAccountingColumns.Count },
                        new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = (ct.PerformanceAccountingColumns.Count * gradeColumnWidth).ToString() }
                    ),
                    new Paragraph(_paragraphProperties.CloneNode(true),
                        new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"), new Text(ct.Name)))
                );
                headerRow2.AppendChild(certificationTypeHeadCell);
            }

            table.AppendChild(headerRow2);

            var headerRow3 = new TableRow();
            var rowProperties = new TableRowProperties();
            rowProperties.Append(new TableRowHeight { Val = 1900 });
            headerRow3.AppendChild(rowProperties);

            var numberEmptyHeadCell2 = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Continue },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _numberColumnWidth.ToString() }
                ),
                new Paragraph(_paragraphProperties.CloneNode(true))
            );
            headerRow3.AppendChild(numberEmptyHeadCell2);

            var studentEmptyHeadCell2 = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Continue },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _studentColumnWidth.ToString() }
                ),
                new Paragraph(_paragraphProperties.CloneNode(true))
            );
            headerRow3.AppendChild(studentEmptyHeadCell2);

            foreach (var ct in certificationTypes)
            {
                foreach (var column in ct.PerformanceAccountingColumns)
                {
                    var subjectNameHeadCell = new TableCell(
                        new Paragraph(_paragraphProperties.CloneNode(true),
                            new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                                new Text(column.Subject == null ? "" : column.Subject.AbbreviatedName)))
                    );
                    var subjectNameHeadCellProperties = cellProperties.CloneNode(true);
                    subjectNameHeadCellProperties.Append(
                        new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = gradeColumnWidth.ToString() },
                        new TextDirection { Val = TextDirectionValues.BottomToTopLeftToRight });
                    subjectNameHeadCell.Append(subjectNameHeadCellProperties);
                    headerRow3.AppendChild(subjectNameHeadCell);
                }
            }

            table.AppendChild(headerRow3);

            foreach (var record in records)
            {
                TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

                TableCell numberCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(fontSize: "26"),
                        new Text(record.Number.ToString() ?? ""))));
                numberCell.Append(new TableCellProperties(
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _numberColumnWidth.ToString() }));
                row.Append(numberCell);

                TableCell studentCell = new TableCell(new Paragraph(new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                    new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text(record.Student == null ? "" : GetStudentFIO(record.Student)))));
                studentCell.Append(new TableCellProperties(
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _studentColumnWidth.ToString() }));
                row.Append(studentCell);

                foreach (var ct in certificationTypes)
                {
                    foreach (var column in ct.PerformanceAccountingColumns)
                    {
                        var grade = record.PerformanceAccountingGrades.FirstOrDefault(g => g.PerformanceAccountingColumnId == column.Id);

                        string gradeText = "";
                        gradeText += grade == null ? "" : ct.Id != 2 ? grade.Grade == null ? "" : grade.Grade.ToString() : 
                            grade.IsPassed == null ? "" : (bool)grade.IsPassed ? "з." : "н.з.";

                        var gradeCell = new TableCell(
                            new TableCellProperties(
                                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = gradeColumnWidth.ToString() }
                            ),
                            new Paragraph(_paragraphProperties.CloneNode(true),
                                new Run(WordUtils.GetRunProperties(fontSize: "26"), new Text(gradeText)))
                        );

                        row.Append(gradeCell);
                    }
                }

                table.Append(row);
            }

            for (int i = records.Count; i < _maxRows; i++) AppendEmptyRow(table);

            _documentBody.Append(table);
        }

        private void AppendEmptyRow(Table table)
        {
            TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

            TableCell numberCell = new TableCell(new Paragraph(_paragraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new Text(""))));
            numberCell.Append(new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _numberColumnWidth.ToString() }));
            row.Append(numberCell);

            TableCell studentCell = new TableCell(new Paragraph(new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                new Run(WordUtils.GetRunProperties(fontSize: "26"),
                new Text(""))));
            studentCell.Append(new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _studentColumnWidth.ToString() }));
            row.Append(studentCell);

            int gradeColumnsCount = GetColumnCount();
            int gradeColumnWidth = _subjectColumnWidth / gradeColumnsCount;
            var certificationTypes = GetNotEmptyCertificationTypes();
            foreach (var ct in certificationTypes)
            {
                foreach (var column in ct.PerformanceAccountingColumns)
                {
                    var gradeCell = new TableCell(
                        new TableCellProperties(
                            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                            new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = gradeColumnWidth.ToString() }
                        ),
                        new Paragraph(_paragraphProperties.CloneNode(true),
                            new Run(WordUtils.GetRunProperties(fontSize: "26"), new Text("")))
                    );

                    row.Append(gradeCell);
                }
            }

            table.Append(row);
        }

        private string GetStudentFIO(Student student)
        {
            return student.LastName + " " + student.FirstName[0] + ". " + student.MiddleName[0] + ".";
        }

        private int GetColumnCount() => _certificationTypes.SelectMany(ct => ct.PerformanceAccountingColumns!).Count();
        private IEnumerable<CertificationType> GetNotEmptyCertificationTypes() => _certificationTypes.Where(ct => ct.PerformanceAccountingColumns!.Count != 0);
    }
}
