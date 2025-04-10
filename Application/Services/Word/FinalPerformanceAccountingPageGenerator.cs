using Application.Utils;
using DataAccess.Interfaces;
using DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent.FinalPerformanceAccounting;
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

        public FinalPerformanceAccountingPageGenerator(int journalId, Body body, IPagesRepository pagesRepository,
            IPerformanceAccountingColumnsRepository columnsRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
            _columnsRepository = columnsRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.FinalPerformanceAccounting);
            if (pages == null) throw new ArgumentException(nameof(pages));

            foreach (var page in pages)
            {
                var certificationTypes = await _columnsRepository.GetByPageIdGroupByCertificationTypes(page.Id);
                if (certificationTypes == null) throw new ArgumentException(nameof(page));
                _certificationTypes = certificationTypes;

                AppendTitle();

                AppendTable(page.FinalPerformanceAccounting);

                WordUtils.AppendPageBreak(_documentBody);
            }
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

            int numberColumnWidth = 150 * 3;
            int studentColumnWidth = 900 * 3;
            int subjectColumnWidth = 3950 * 3;

            int gradeColumnsCount = GetColumnCount();
            int gradeColumnWidth = subjectColumnWidth / gradeColumnsCount;

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

            var headerRow1 = new TableRow();

            var numberHeadCell = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Restart },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = numberColumnWidth.ToString() }
                ),
                new Paragraph(new Run(WordUtils.GetRunProperties(bold: true), new Text("№")))
            );
            headerRow1.AppendChild(numberHeadCell);

            var studentHeadCell = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Restart },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = studentColumnWidth.ToString() }
                ),
                new Paragraph(new Run(WordUtils.GetRunProperties(bold: true), new Text("Ф. И. О. студента")))
            );
            headerRow1.AppendChild(studentHeadCell);

            var subjectHeadCell = new TableCell(
                new TableCellProperties(
                    new GridSpan { Val = gradeColumnsCount },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = subjectColumnWidth.ToString() }
                ),
                new Paragraph(new Run(WordUtils.GetRunProperties(bold: true), new Text("Название дисциплины")))
            );
            headerRow1.AppendChild(subjectHeadCell);

            table.AppendChild(headerRow1);

            var headerRow2 = new TableRow();

            var numberEmptyHeadCell1 = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Continue },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = numberColumnWidth.ToString() }
                ),
                new Paragraph()
            );
            headerRow2.AppendChild(numberEmptyHeadCell1);

            var studentEmptyHeadCell1 = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Continue },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = studentColumnWidth.ToString() }
                ),
                new Paragraph()
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
                    new Paragraph(new Run(WordUtils.GetRunProperties(bold: true), new Text(ct.Name)))
                );
                headerRow2.AppendChild(certificationTypeHeadCell);
            }

            table.AppendChild(headerRow2);

            var headerRow3 = new TableRow();

            var numberEmptyHeadCell2 = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Continue },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = numberColumnWidth.ToString() }
                ),
                new Paragraph()
            );
            headerRow3.AppendChild(numberEmptyHeadCell2);

            var studentEmptyHeadCell2 = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Continue },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = studentColumnWidth.ToString() }
                ),
                new Paragraph()
            );
            headerRow3.AppendChild(studentEmptyHeadCell2);

            foreach (var ct in certificationTypes)
            {
                foreach (var column in ct.PerformanceAccountingColumns)
                {
                    var subjectNameHeadCell = new TableCell(
                        new TableCellProperties(
                            new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = (gradeColumnWidth).ToString() }
                        ),
                        new Paragraph(
                            new Run(WordUtils.GetRunProperties(bold: true),
                                new Text(column.Subject == null ? "" : column.Subject.AbbreviatedName)))
                    );
                    headerRow3.AppendChild(subjectNameHeadCell);
                }
            }

            table.AppendChild(headerRow3);

            //foreach (var record in records)
            //{
            //    TableRow row = new TableRow();

            //    TableCell keyIndicatorNameCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(),
            //        new Text(IndicatorsNames[record.KeyIndicatorId]))));
            //    keyIndicatorNameCell.Append(new TableCellProperties(
            //        new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = numberColumnWidth.ToString() }));

            //    row.Append(keyIndicatorNameCell);

            //    foreach (var i in record.KeyIndicatorsByCourse)
            //    {
            //        string indicatorStr = "";
            //        if (record.KeyIndicatorId != 5)
            //            indicatorStr = i.Value == null ? "" : ((int)i.Value).ToString();
            //        else
            //            indicatorStr = i.Value == null ? "" : ((double)i.Value).ToString();

            //        var indicatorByCourseCell = new TableCell(
            //                new TableCellProperties(
            //                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = (studentColumnWidth / _courses.Count).ToString() }
            //                ),
            //                new Paragraph(new Run(WordUtils.GetRunProperties(), new Text(indicatorStr)))
            //            );

            //        row.Append(indicatorByCourseCell);
            //    }

            //    TableCell noteValueCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(),
            //        new Text(record.Note == null ? "" : record.Note))));
            //    noteValueCell.Append(new TableCellProperties(
            //        new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = subjectColumnWidth.ToString() }));

            //    row.Append(noteValueCell);
            //    table.Append(row);
            //}

            _documentBody.Append(table);
        }

        private int GetColumnCount() => _certificationTypes.SelectMany(ct => ct.PerformanceAccountingColumns!).Count();
        private IEnumerable<CertificationType> GetNotEmptyCertificationTypes() => _certificationTypes.Where(ct => ct.PerformanceAccountingColumns!.Count != 0);
    }
}
