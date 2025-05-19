using Application.Entities;
using Application.Utils;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.Json;

namespace Application.Services.Word
{

    public class JournalKeepingPageGenerator(Body body)
    {
        private readonly Body _documentBody = body;

        public async Task Generate()
        {
            string journalKeepingFilePath = "JournalsData/JournalKeeping.json";
            if (!File.Exists(journalKeepingFilePath)) throw new FileNotFoundException(journalKeepingFilePath);
            using FileStream journalKeepingStream = File.OpenRead(journalKeepingFilePath);
            var journalKeeping = await JsonSerializer.DeserializeAsync<JournalKeeping>(journalKeepingStream);
            if (journalKeeping == null) throw new FileFormatException(journalKeepingFilePath);

            AppendTitle(journalKeeping.Title);
            AppendContent(journalKeeping.Content);
            WordUtils.AppendPageBreak(_documentBody);
        }

        private void AppendTitle(string title)
        {
            var titleParagraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { After = "250" });
            var titleParagraph = new Paragraph(titleParagraphProperties);

            var titleRun = new Run(WordUtils.GetRunProperties(bold: true),
                new Text(title));

            titleParagraph.Append(titleRun);
            _documentBody.Append(titleParagraph);
        }

        private void AppendContent(string content)
        {
            var tabs = new Tabs(new TabStop()
            {
                Val = TabStopValues.Left,
                Position = 360
            });
            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both },
                new SpacingBetweenLines { After = "0", Before = "0", Line = "240" }, tabs);


            var paragraphsStrs = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var paragraphStr in paragraphsStrs)
            {
                var paragraph = new Paragraph(paragraphProperties.CloneNode(true));
                var run = new Run(WordUtils.GetRunProperties(fontSize: "26"),
                    new TabChar(), new Text(paragraphStr));

                paragraph.Append(run);
                _documentBody.Append(paragraph);
            }
        }
    }
}
