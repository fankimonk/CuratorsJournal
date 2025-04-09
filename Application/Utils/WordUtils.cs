using DocumentFormat.OpenXml.Wordprocessing;

namespace Application.Utils
{
    public static class WordUtils
    {
        private static SectionProperties s_PortraitSectionProperties = new SectionProperties(
            new PageSize() { Width = 11906, Height = 16838, Orient = PageOrientationValues.Portrait });

        private static SectionProperties s_LandscapeSectionProperties = new SectionProperties(
            new PageSize() { Width = 16838, Height = 11906, Orient = PageOrientationValues.Landscape });

        public enum PageOrientationTypes
        {
            Portrait = 1,
            Landscape = 2
        }

        public static void AppendSectionBreak(PageOrientationTypes pageOrientationType, Body body)
        {
            SectionProperties sectionProperties = new();

            switch (pageOrientationType)
            {
                case PageOrientationTypes.Portrait:
                    sectionProperties = new(s_PortraitSectionProperties.CloneNode(true) as SectionProperties);
                    break;

                case PageOrientationTypes.Landscape:
                    sectionProperties = new(s_LandscapeSectionProperties.CloneNode(true) as SectionProperties);
                    break;
            }

            body.Append(new Paragraph(new ParagraphProperties(sectionProperties)));
        }

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
