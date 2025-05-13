using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.Holidays;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;
using Frontend.Utils;
using static Azure.Core.HttpHeader;

namespace Application.Services.Word
{
    public class HolidaysPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;
        private readonly IHolidaysRepository _holidaysRepository;

        private readonly Body _documentBody;

        private readonly int _journalId;

        private readonly int _dateColumnWidth = 3500;
        private readonly int _nameColumnWidth = 6500;

        private readonly int _maxLines = 48;
        private readonly int _titleMaxLineChars = 65;
        private readonly int _nameMaxLineChars = 58;

        private int _linesCount = 0;

        private readonly TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellMargin(
                new TopMargin { Width = "0", Type = TableWidthUnitValues.Dxa },
                new BottomMargin { Width = "0", Type = TableWidthUnitValues.Dxa },
                new LeftMargin { Width = "50", Type = TableWidthUnitValues.Dxa },
                new RightMargin { Width = "0", Type = TableWidthUnitValues.Dxa })
        );

        public HolidaysPageGenerator(int journalId, Body body, IPagesRepository pagesRepository, IHolidaysRepository holidaysRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
            _holidaysRepository = holidaysRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.Holidays);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                await GeneratePage(page);
            }
            else
            {
                foreach (var p in pages)
                {
                    _linesCount = 0;
                    await GeneratePage(p);
                    WordUtils.AppendPageBreak(_documentBody);
                }
            }   
        }

        private async Task GeneratePage(Page page)
        {
            var holidayTypes = await _holidaysRepository.GetByPageIdAsync(page.Id);
            if (holidayTypes == null) throw new ArgumentException(nameof(page));

            var notEmptyHolidayTypes = holidayTypes.Where(ht => ht.Holidays.Count > 0);
            foreach (var ht in notEmptyHolidayTypes)
            {
                if (_linesCount + CalculateTitleLinesCount(ht.Name) + 1 > _maxLines) break;
                var table = AppendTable(ht);
                if (ht == notEmptyHolidayTypes.Last())
                {
                    for (int i = CalculateLineCount(notEmptyHolidayTypes.ToList()); i < _maxLines; i++)
                    {
                        AppendEmptyRow(table);
                    }
                }
            }
        }

        private void AppendTitleName(string text)
        {
            var runProperties = new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new FontSize() { Val = "28" },
                new Bold()
            );

            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines { After = "100", Before = "100" }),
                new Run(
                    runProperties,
                    new Text(text))
            );

            _documentBody.Append(title);
        }

        private Table AppendTable(HolidayType holidayType)
        {
            _linesCount += CalculateTitleLinesCount(holidayType.Name);

            AppendTitleName(holidayType.Name);

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
                new GridColumn() { Width = _dateColumnWidth.ToString() },
                new GridColumn() { Width = _nameColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            foreach (var holiday in holidayType.Holidays)
            {
                string nameStr = holiday.Name ?? "";
                int nameLines = CalculateNameLinesCount(nameStr);
                if (_linesCount + nameLines > _maxLines) break;
                _linesCount += nameLines;

                TableRow row = new TableRow();

                TableRowProperties rowProperties = new TableRowProperties(
                    new TableRowHeight { Val = 0, HeightType = HeightRuleValues.Auto }
                );
                row.Append(rowProperties);

                var dateRunProperties = new RunProperties(
                    new RunFonts()
                    {
                        Ascii = "Times New Roman",
                        HighAnsi = "Times New Roman",
                        EastAsia = "Times New Roman",
                        ComplexScript = "Times New Roman"
                    },
                    new FontSize() { Val = "22" }
                );

                string dateText = "";
                if (holiday.Month != null)
                {
                    if (holiday.IsRelativeDate) dateText = holiday.RelativeDate == null ? "" : holiday.RelativeDate + " ";
                    else dateText = holiday.Day == null ? "" : holiday.Day.ToString() + " ";
                    dateText += MonthsUtils.MonthsDateNames[(int)holiday.Month];
                }

                TableCell dateCell = new TableCell(new Paragraph(
                    new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                    new Run(dateRunProperties, new Text(dateText))));
                var dateCellProperties = _cellProperties.CloneNode(true);
                dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() });
                dateCell.Append(dateCellProperties);

                var nameRunProperties = new RunProperties(
                    new RunFonts()
                    {
                        Ascii = "Times New Roman",
                        HighAnsi = "Times New Roman",
                        EastAsia = "Times New Roman",
                        ComplexScript = "Times New Roman"
                    },
                    new FontSize() { Val = "22" }
                );

                TableCell nameCell = new TableCell(new Paragraph(
                    new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                    new Run(nameRunProperties, new Text(nameStr))));
                var nameCellProperties = _cellProperties.CloneNode(true);
                nameCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _nameColumnWidth.ToString() });
                nameCell.Append(nameCellProperties);

                row.Append(dateCell, nameCell);
                table.Append(row);
            }

            _documentBody.Append(table);
            return table;
        }

        private void AppendEmptyRow(Table table)
        {
            TableRow row = new TableRow();

            TableRowProperties rowProperties = new TableRowProperties(
                new TableRowHeight { Val = 0, HeightType = HeightRuleValues.Auto }
            );
            row.Append(rowProperties);

            TableCell dateCell = new TableCell(new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                new Run(WordUtils.GetRunProperties(fontSize: "22"), new Text(""))));
            var dateCellProperties = _cellProperties.CloneNode(true);
            dateCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _dateColumnWidth.ToString() });
            dateCell.Append(dateCellProperties);

            TableCell nameCell = new TableCell(new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                new Run(WordUtils.GetRunProperties(fontSize: "22"), new Text(""))));
            var nameCellProperties = _cellProperties.CloneNode(true);
            nameCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _nameColumnWidth.ToString() });
            nameCell.Append(nameCellProperties);

            row.Append(dateCell, nameCell);
            table.Append(row);
        }

        private int CalculateLineCount(List<HolidayType> holidayTypes)
        {
            int lineCount = 0;
            foreach (var ht in holidayTypes.Where(ht => ht.Holidays.Count > 0))
            {
                int titleLines = ht.Name.Length / _titleMaxLineChars;
                titleLines += ht.Name.Length % _titleMaxLineChars == 0 ? 0 : 1;
                lineCount += 2 + titleLines - 1;
                foreach (var holiday in ht.Holidays)
                {
                    string name = holiday.Name ?? "";
                    int nameLinesCount = name.Length / _nameMaxLineChars + 1;
                    lineCount += nameLinesCount;
                }
            }
            return lineCount;
        }

        private int CalculateAverageLines(List<HolidayType> holidayTypes) =>
            CalculateLineCount(holidayTypes) / holidayTypes.Count;

        private int CalculateTitleLinesCount(string title)
        {
            int titleLines = title.Length / _titleMaxLineChars;
            titleLines += title.Length % _titleMaxLineChars == 0 ? 0 : 1;
            return 2 + titleLines - 1;
        }

        private int CalculateNameLinesCount(string name)
        {
            int nameLinesCount = name.Length / _nameMaxLineChars + 1;
            return nameLinesCount;
        }
    }
}
