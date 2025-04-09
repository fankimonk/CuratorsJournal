using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class ContactPhonesPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public ContactPhonesPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.ContactPhones);
            if (pages == null) throw new ArgumentException(nameof(pages));
            foreach (var page in pages)
            {
                AppendTitleName();
                WordUtils.AppendBreaks(1, _documentBody);

                AppendTable(page.ContactPhoneNumbers);

                WordUtils.AppendPageBreak(_documentBody);
            }
        }

        private void AppendTitleName()
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
                    new Text("КОНТАКТНЫЕ ТЕЛЕФОНЫ"))
            );

            _documentBody.Append(title);
        }

        private void AppendTable(List<ContactPhoneNumber> contactPhones)
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

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = "5000" },
                new GridColumn() { Width = "5000" }
            );
            table.AppendChild(tableGrid);

            foreach (var phone in contactPhones)
            {
                TableRow row = new TableRow();

                var nameRunProperties = new RunProperties(
                    new RunFonts()
                    {
                        Ascii = "Times New Roman",
                        HighAnsi = "Times New Roman",
                        EastAsia = "Times New Roman",
                        ComplexScript = "Times New Roman"
                    },
                    new FontSize() { Val = "28" }
                );

                TableCell nameCell = new TableCell(new Paragraph(new Run(nameRunProperties, new Text(phone.Name ?? ""))));
                nameCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "5000" }));

                var phoneRunProperties = new RunProperties(
                    new RunFonts()
                    {
                        Ascii = "Times New Roman",
                        HighAnsi = "Times New Roman",
                        EastAsia = "Times New Roman",
                        ComplexScript = "Times New Roman"
                    },
                    new FontSize() { Val = "28" }
                );

                TableCell phoneCell = new TableCell(new Paragraph(new Run(phoneRunProperties, new Text(phone.PhoneNumber ?? ""))));
                phoneCell.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "5000" }));

                row.Append(nameCell, phoneCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }
    }
}
