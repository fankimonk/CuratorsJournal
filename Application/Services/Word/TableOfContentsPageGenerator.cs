using Application.Utils;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Application.Services.Word
{
    public class TableOfContentsPageGenerator(Body body)
    {
        private readonly Body _documentBody = body;

        public void Generate()
        {
            AppendTitle();
            AppendContent();
            WordUtils.AppendPageBreak(_documentBody);
        }

        private void AppendTitle()
        {
            var titleParagraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { After = "250" });
            var titleParagraph = new Paragraph(titleParagraphProperties);

            var titleRun = new Run(WordUtils.GetRunProperties(bold: true),
                new Text("СОДЕРЖАНИЕ"));

            titleParagraph.Append(titleRun);
            _documentBody.Append(titleParagraph);
        }

        private void AppendContent()
        {

        }
    }
}
