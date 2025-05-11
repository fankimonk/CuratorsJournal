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
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.DynamicsOfKeyIndicators);
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

            TableCellProperties cellProperties = new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Top }
            );

            TableCellMargin cellMargin = new TableCellMargin(
                new TopMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
                new BottomMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
                new LeftMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
                new RightMargin { Width = "100", Type = TableWidthUnitValues.Dxa }
            );

            cellProperties.Append(cellMargin);

            ParagraphProperties paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { Before = "0", After = "0" });

            var headerRow1 = new TableRow();

            var keyIndicatorsHeadCellProperties = cellProperties.CloneNode(true);
            keyIndicatorsHeadCellProperties.Append(new VerticalMerge { Val = MergedCellValues.Restart },
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = keyIndicatorsColumnWidth.ToString() });

            var keyIndicatorsCell = new TableCell(
                keyIndicatorsHeadCellProperties,
                new Paragraph(
                    paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"), new Text("Основные показатели")))
            );
            headerRow1.AppendChild(keyIndicatorsCell);

            var courseHeadCellProperties = cellProperties.CloneNode(true);
            courseHeadCellProperties.Append(new GridSpan { Val = _courses.Count },
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = coursesColumnWidth.ToString() });

            var courseCell = new TableCell(
                courseHeadCellProperties,
                new Paragraph(paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"), new Text("Курс")))
            );
            headerRow1.AppendChild(courseCell);

            var noteHeadCellProperties = cellProperties.CloneNode(true);
            noteHeadCellProperties.Append(new VerticalMerge { Val = MergedCellValues.Restart },
                new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = noteColumnWidth.ToString() });

            var noteCell = new TableCell(
                noteHeadCellProperties,
                new Paragraph(paragraphProperties.CloneNode(true),
                    new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"), new Text("Примечание")))
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

            var courseValueHeadCellProperties = cellProperties.CloneNode(true);
            courseValueHeadCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = (coursesColumnWidth / _courses.Count).ToString() });

            foreach (var course in _courses)
            {
                var courseHeaderCell = new TableCell(
                    courseValueHeadCellProperties.CloneNode(true),
                    new Paragraph(paragraphProperties.CloneNode(true),
                        new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"), new Text(course.ToString())))
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

                TableCell keyIndicatorNameCell = new TableCell(new Paragraph(
                    new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                    new Run(WordUtils.GetRunProperties(fontSize: "26"),
                        new Text(IndicatorsNames[record.KeyIndicatorId]))));
                keyIndicatorNameCell.Append(keyIndicatorsHeadCellProperties.CloneNode(true));

                row.Append(keyIndicatorNameCell);

                foreach (var i in record.KeyIndicatorsByCourse)
                {
                    string indicatorStr = "";
                    if (record.KeyIndicatorId != 5)
                        indicatorStr = i.Value == null ? "" : ((int)i.Value).ToString();
                    else
                        indicatorStr = i.Value == null ? "" : ((double)i.Value).ToString();

                    TableCellProperties indicatorByCourseCellProperties = new TableCellProperties(
                        new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
                    );

                    var indicatorByCourseCell = new TableCell(
                            indicatorByCourseCellProperties.CloneNode(true),
                            new Paragraph(paragraphProperties.CloneNode(true),
                                new Run(WordUtils.GetRunProperties(fontSize: "26"), new Text(indicatorStr)))
                        );

                    row.Append(indicatorByCourseCell);
                }

                TableCell noteValueCell = new TableCell(new Paragraph(
                    new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                    new Run(WordUtils.GetRunProperties(fontSize: "26"),
                        new Text(record.Note == null ? "" : record.Note))));
                noteValueCell.Append(noteHeadCellProperties.CloneNode(true));

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
