using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class ContactPhonesPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private readonly int _journalId;

        private readonly int _maxLineChars = 35;
        private readonly int _maxLines = 35;

        private readonly int _nameColumnWidth = 5000;
        private readonly int _phoneColumnWidth = 5000;

        private readonly UInt32Value _valueRowHeight = 385;

        private readonly TableCellProperties _cellProperties = new TableCellProperties(
            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
            new TableCellMargin(
                //new TopMargin { Width = "25", Type = TableWidthUnitValues.Dxa },
                //new BottomMargin { Width = "25", Type = TableWidthUnitValues.Dxa },
                new LeftMargin { Width = "100", Type = TableWidthUnitValues.Dxa },
                new RightMargin { Width = "100", Type = TableWidthUnitValues.Dxa }
            )
        );

        public ContactPhonesPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.ContactPhones);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                GeneratePage(page);
            }
            else
            {
                foreach (var p in pages)
                {
                    GeneratePage(p);
                    WordUtils.AppendPageBreak(_documentBody);
                }
            }
        }

        private void GeneratePage(Page page)
        {
            AppendTitleName();

            AppendTable(page.ContactPhoneNumbers);
        }

        private void AppendTitleName()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines { After = "0", Before = "0" }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("КОНТАКТНЫЕ ТЕЛЕФОНЫ"), new Break())
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
                new GridColumn() { Width = _nameColumnWidth.ToString() },
                new GridColumn() { Width = _phoneColumnWidth.ToString() }
            );
            table.AppendChild(tableGrid);

            int linesCount = 0;
            foreach (var phone in contactPhones)
            {
                var nameStr = phone.Name ?? "";
                int nameLinesCount = nameStr.Length / _maxLineChars + 1;
                if (linesCount + nameLinesCount > _maxLines) break;
                linesCount += nameLinesCount;

                TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

                TableCell nameCell = new TableCell(new Paragraph(
                    new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                    new Run(WordUtils.GetRunProperties(fontSize: "26"), new Text(nameStr))));
                var nameCellProperties = _cellProperties.CloneNode(true);
                nameCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _nameColumnWidth.ToString() });
                nameCell.Append(nameCellProperties);

                TableCell phoneCell = new TableCell(new Paragraph(
                    new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                    new Run(WordUtils.GetRunProperties(fontSize: "26"), new Text(phone.PhoneNumber ?? ""))));
                var phoneCellProperties = _cellProperties.CloneNode(true);
                phoneCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _nameColumnWidth.ToString() });
                phoneCell.Append(phoneCellProperties);

                row.Append(nameCell, phoneCell);
                table.Append(row);
            }

            for (int i = linesCount; i < _maxLines; i++) AppendEmptyRow(table);

            _documentBody.Append(table);
        }

        private void AppendEmptyRow(Table table)
        {
            TableRow row = new TableRow(new TableRowProperties(new TableRowHeight() { Val = _valueRowHeight }));

            //TableRowProperties rowProperties = new TableRowProperties(
            //    new TableRowHeight { Val = 0, HeightType = HeightRuleValues.Auto }
            //);
            //row.Append(rowProperties);

            TableCell nameCell = new TableCell(new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                new Run(WordUtils.GetRunProperties(fontSize: "26"), new Text(""))));
            var nameCellProperties = _cellProperties.CloneNode(true);
            nameCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _nameColumnWidth.ToString() });
            nameCell.Append(nameCellProperties);

            TableCell phoneCell = new TableCell(new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { Before = "0", After = "0" }),
                new Run(WordUtils.GetRunProperties(fontSize: "26"), new Text(""))));
            var phoneCellProperties = _cellProperties.CloneNode(true);
            phoneCellProperties.Append(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = _nameColumnWidth.ToString() });
            phoneCell.Append(phoneCellProperties);

            row.Append(nameCell, phoneCell);
            table.Append(row);
        }
    }
}
