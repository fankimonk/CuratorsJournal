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
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines { After = "0" }),
                new Run(
                    runProperties,
                    new Text("КОНТАКТНЫЕ ТЕЛЕФОНЫ"),
                    new Break())
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

            TableCellProperties cellProperties = new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
            );

            TableCellMargin cellMargin = new TableCellMargin(
                new TopMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
                new BottomMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
                new LeftMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
                new RightMargin { Width = "100", Type = TableWidthUnitValues.Dxa }
            );

            cellProperties.Append(cellMargin);

            table.AppendChild(tblProperties);

            int nameColumnWidth = 5000;
            int phoneColumnWidth = 5000;

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = nameColumnWidth.ToString() },
                new GridColumn() { Width = phoneColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            foreach (var phone in contactPhones)
            {
                TableRow row = new TableRow();

                TableRowProperties rowProperties = new TableRowProperties(
                    new TableRowHeight { Val = 0, HeightType = HeightRuleValues.Auto }
                );
                row.Append(rowProperties);

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

                TableCell nameCell = new TableCell(new Paragraph(
                    new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                    new Run(nameRunProperties, new Text(phone.Name ?? ""))));
                var nameCellProperties = cellProperties.CloneNode(true);
                nameCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = nameColumnWidth.ToString() });
                nameCell.Append(nameCellProperties);

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

                TableCell phoneCell = new TableCell(new Paragraph(
                    new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                    new Run(phoneRunProperties, new Text(phone.PhoneNumber ?? ""))));
                var phoneCellProperties = cellProperties.CloneNode(true);
                phoneCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = nameColumnWidth.ToString() });
                phoneCell.Append(phoneCellProperties);

                row.Append(nameCell, phoneCell);
                table.Append(row);
            }

            _documentBody.Append(table);
        }
    }
}
