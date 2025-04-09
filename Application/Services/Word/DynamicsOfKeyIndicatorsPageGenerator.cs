using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class DynamicsOfKeyIndicatorsPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        private List<int> _courses = [];

        public DynamicsOfKeyIndicatorsPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.DynamicsOfKeyIndicators);
            if (pages == null) throw new ArgumentException(nameof(pages));

            foreach (var page in pages)
            {
                _courses = GetCourses(page.DynamicsOfKeyIndicators);

                AppendTitle();

                AppendTable(page.DynamicsOfKeyIndicators);

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
                    new Text("ДИНАМИКА ОСНОВНЫХ ПОКАЗАТЕЛЕЙ ГРУППЫ"),
                    new Break()),
                new Run(
                    WordUtils.GetRunProperties(bold: true),
                    new Text("ЗА ПЕРИОД ОБУЧЕНИЯ"))
            );

            _documentBody.Append(title);
        }

        private void AppendTable(List<DynamicsOfKeyIndicatorsRecord> dynamicsOfKeyIndicators)
        {
            var table = new Table();

            int keyIndicatorsColumnWidth = 4050;
            int coursesColumnWidth = 3450;
            int noteColumnWidth = 2500;

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

            var keyIndicatorsCell = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Restart },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = keyIndicatorsColumnWidth.ToString() }
                ),
                new Paragraph(new Run(WordUtils.GetRunProperties(bold: true), new Text("Основные показатели")))
            );
            headerRow1.AppendChild(keyIndicatorsCell);

            var courseCell = new TableCell(
                new TableCellProperties(
                    new GridSpan { Val = _courses.Count },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = coursesColumnWidth.ToString() }
                ),
                new Paragraph(new Run(WordUtils.GetRunProperties(bold: true), new Text("Курс")))
            );
            headerRow1.AppendChild(courseCell);

            var noteCell = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Restart },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() }
                ),
                new Paragraph(new Run(WordUtils.GetRunProperties(bold: true), new Text("Примечание")))
            );
            headerRow1.AppendChild(noteCell);

            table.AppendChild(headerRow1);

            var headerRow2 = new TableRow();

            var keyIndicatorsEmptyCell = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Continue },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = keyIndicatorsColumnWidth.ToString() }
                ),
                new Paragraph()
            );
            headerRow2.AppendChild(keyIndicatorsEmptyCell);

            foreach (var course in _courses)
            {
                var courseHeaderCell = new TableCell(
                    new TableCellProperties(
                        new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = (coursesColumnWidth / _courses.Count).ToString() }
                    ),
                    new Paragraph(new Run(WordUtils.GetRunProperties(bold: true), new Text(course.ToString())))
                );
                headerRow2.AppendChild(courseHeaderCell);
            }

            var noteEmptyCell = new TableCell(
                new TableCellProperties(
                    new VerticalMerge { Val = MergedCellValues.Continue },
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() }
                ),
                new Paragraph()
            );
            headerRow2.AppendChild(noteEmptyCell);

            table.AppendChild(headerRow2);

            foreach (var record in dynamicsOfKeyIndicators)
            {
                TableRow row = new TableRow();

                TableCell keyIndicatorNameCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(),
                    new Text(IndicatorsNames[record.KeyIndicatorId]))));
                keyIndicatorNameCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = keyIndicatorsColumnWidth.ToString() }));

                row.Append(keyIndicatorNameCell);

                foreach (var i in record.KeyIndicatorsByCourse)
                {
                    string indicatorStr = "";
                    if (record.KeyIndicatorId != 5)
                        indicatorStr = i.Value == null ? "" : ((int)i.Value).ToString();
                    else
                        indicatorStr = i.Value == null ? "" : ((double)i.Value).ToString();

                    var indicatorByCourseCell = new TableCell(
                            new TableCellProperties(
                                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = (coursesColumnWidth / _courses.Count).ToString() }
                            ),
                            new Paragraph(new Run(WordUtils.GetRunProperties(), new Text(indicatorStr)))
                        );

                    row.Append(indicatorByCourseCell);
                }

                TableCell noteValueCell = new TableCell(new Paragraph(new Run(WordUtils.GetRunProperties(),
                    new Text(record.Note == null ? "" : record.Note))));
                noteValueCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() }));

                row.Append(noteValueCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }

        private static List<int> GetCourses(List<DynamicsOfKeyIndicatorsRecord> dynamicsOfKeyIndicators)
        {
            return dynamicsOfKeyIndicators
                .Select(r => r.KeyIndicatorsByCourse
                .Select(r => r.Course))
                .SelectMany(r => r)
                .GroupBy(r => r)
                .Select(r => r.Key).ToList();
        }

        private Dictionary<int, string> IndicatorsNames = new Dictionary<int, string>
        {
            { 1, "Общее количество студентов"},
            { 2, "Отчислено студентов" },
            { 3, "Восстановлено" },
            { 4, "Переведено с платной на бюджетную форму обучения, снижена плата за обучение" },
            { 5, "Средний балл успеваемости" },
            { 6, "Количество студентов-отличников" }
        };
    }
}
