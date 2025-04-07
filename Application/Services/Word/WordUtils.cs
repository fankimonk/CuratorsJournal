using DocumentFormat.OpenXml.Wordprocessing;

namespace Application.Services.Word
{
    public static class WordUtils
    {
        public static void AppendPageBreak(Body body)
        {
            body.Append(new Paragraph(
                new Run(new Break() { Type = BreakValues.Page })));
        }

        public static void AppendBreaks(int count, Body body)
        {
            var run = new Run(new RunProperties(
                new RunFonts()
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new FontSize() { Val = "28" }
            ));

            for (int i = 0; i < count - 1; i++)
            {
                run.Append(new Break());
            }

            body.Append(new Paragraph(run));
        }

        public static RunProperties GetRunProperties(bool underline = false, bool bold = false, string font = "Times New Roman", string fontSize = "28")
        {
            var props = new RunProperties(
                new RunFonts()
                {
                    Ascii = font,
                    HighAnsi = font,
                    EastAsia = font,
                    ComplexScript = font
                },
                new FontSize() { Val = fontSize }
            );

            if (underline)
                props.Append(new Underline() { Val = UnderlineValues.Single });

            if (bold)
                props.Append(new Bold());

            return props;
        }
    }
}
