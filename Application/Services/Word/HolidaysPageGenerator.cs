using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.Holidays;
using Domain.Enums.Journal;
using Frontend.Utils;

namespace Application.Services.Word
{
    public class HolidaysPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;
        private readonly IHolidaysRepository _holidaysRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public HolidaysPageGenerator(int journalId, Body body, IPagesRepository pagesRepository, IHolidaysRepository holidaysRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
            _holidaysRepository = holidaysRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.Holidays);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                var holidayTypes = await _holidaysRepository.GetByPageIdAsync(page.Id);
                if (holidayTypes == null) throw new ArgumentException(nameof(page));

                foreach (var ht in holidayTypes.Where(ht => ht.Holidays.Count > 0))
                {
                    AppendTable(ht);
                }
                
                WordUtils.AppendPageBreak(_documentBody);
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
                    new Justification { Val = JustificationValues.Center }),
                new Run(
                    runProperties,
                    new Text(text))
            );

            _documentBody.Append(title);
        }

        private void AppendTable(HolidayType holidayType)
        {
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
                new GridColumn() { Width = "3000" },
                new GridColumn() { Width = "7000" }
            );
            table.AppendChild(tableGrid);

            foreach (var holiday in holidayType.Holidays)
            {
                TableRow row = new TableRow();

                var dateRunProperties = new RunProperties(
                    new RunFonts()
                    {
                        Ascii = "Times New Roman",
                        HighAnsi = "Times New Roman",
                        EastAsia = "Times New Roman",
                        ComplexScript = "Times New Roman"
                    },
                    new FontSize() { Val = "24" }
                );

                string dateText = "";
                if (holiday.Month != null)
                {
                    if (holiday.IsRelativeDate) dateText = holiday.RelativeDate == null ? "" : holiday.RelativeDate + " ";
                    else dateText = holiday.Day == null ? "" : holiday.Day.ToString() + " ";
                    dateText += MonthsUtils.MonthsDateNames[(int)holiday.Month];
                }

                TableCell dateCell = new TableCell(new Paragraph(new Run(dateRunProperties, new Text(dateText))));
                dateCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "3000" }));

                var nameRunProperties = new RunProperties(
                    new RunFonts()
                    {
                        Ascii = "Times New Roman",
                        HighAnsi = "Times New Roman",
                        EastAsia = "Times New Roman",
                        ComplexScript = "Times New Roman"
                    },
                    new FontSize() { Val = "24" }
                );

                TableCell nameCell = new TableCell(new Paragraph(new Run(nameRunProperties, new Text(holiday.Name ?? ""))));
                nameCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "7000" }));

                row.Append(dateCell, nameCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }
    }
}
