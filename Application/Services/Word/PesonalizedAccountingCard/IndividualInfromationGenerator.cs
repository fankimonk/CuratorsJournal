using Application.Utils;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Application.Services.Word.PesonalizedAccountingCard
{
    public class IndividualInfromationGenerator
    {
        private readonly List<IndividualInformationRecord> _individualInformation;

        private readonly Body _documentBody;

        private readonly int _maxLineChars = 86;
        private readonly int _maxLines = 5;
        private int _totalLines = 0;

        public IndividualInfromationGenerator(List<IndividualInformationRecord> individualInformation, Body body)
        {
            _individualInformation = individualInformation;
            _documentBody = body;
        }

        public void Generate()
        {
            AppendTitle();
            AppendContent();
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Start },
                    new SpacingBetweenLines() { After = "0" } ),
                new Run(WordUtils.GetRunProperties(bold: true, fontSize: "26"),
                    new Text("Индивидуальные сведения"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(fontSize: "20"),
                    new Text("(участие в научной работе, олимпиадах, студенческих конференциях, спортивной"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(fontSize: "20"),
                    new Text("и общественной жизни вуза, факультета, группы, общежития, ПО ОО \"БРСМ\", и т.д.)"))
            );

            _documentBody.Append(title);
        }

        private void AppendContent()
        {
            var content = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both }));

            
            foreach (var record in _individualInformation)
            {
                string text = "";
                text += record.ActivityName == null ? "" : record.ActivityName + ", ";
                text += record.ParticipationKind == null ? "" : record.ParticipationKind + ", ";

                string period = "";
                period += record.StartDate != null && record.EndDate != null ? record.StartDate.ToString() + "-" + record.EndDate.ToString()
                    : record.StartDate != null ? record.StartDate.ToString() : record.EndDate != null ? record.EndDate.ToString() : "";
                text += period + ", ";

                text += record.Result == null ? "" : record.Result + ", ";
                text += record.Note == null ? "" : record.Note + "; ";

                AppendRecord(text);
            }

            AppendEmptyLines(_maxLines - _totalLines);
        }

        private void AppendRecord(string text)
        {
            var spacing = new SpacingBetweenLines() { Before = "0", After = "0" };
            var paragraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }, spacing);

            var paragraph = new Paragraph(paragraphProperties.CloneNode(true));
            var run = new Run(WordUtils.GetRunProperties(underline: true, fontSize: "24"));

            if (text != "")
            {
                var split = text.Split(' ');
                var lines = new List<string>();
                string currentLine = "";

                foreach (var word in split)
                {
                    if (currentLine.Length + word.Length + 1 <= _maxLineChars)
                    {
                        currentLine += word + " ";
                    }
                    else
                    {
                        lines.Add(currentLine);
                        currentLine = word + " ";
                    }
                }

                if (!string.IsNullOrEmpty(currentLine) && !lines.Contains(currentLine))
                {
                    lines.Add(currentLine);
                }

                if (_totalLines + lines.Count > _maxLines) return;
                _totalLines += lines.Count;

                for (int i = 0; i < lines.Count; i++)
                {
                    var lineStr = lines[i];

                    int tabCount = 13 - (Math.Max(0, lineStr.Length - 1) / 7);

                    var newParagraph = paragraph.CloneNode(true);
                    var newValueRun = run.CloneNode(true);
                    newValueRun.Append(new Text(lineStr));

                    for (int j = 0; j < tabCount; j++)
                    {
                        newValueRun.Append(new TabChar());
                    }

                    newParagraph.Append(newValueRun);
                    _documentBody.Append(newParagraph);
                }
            }
        }

        private void AppendEmptyLines(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var emptyParagraph = new Paragraph(new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both },
                    new SpacingBetweenLines() { Before = "0", After = "0" }
                ));
                var emptyRun = new Run(WordUtils.GetRunProperties(underline: true));
                for (int j = 0; j < 13; j++)
                {
                    emptyRun.Append(new TabChar());
                }
                emptyParagraph.Append(emptyRun);
                _documentBody.Append(emptyParagraph);
            }
        }
    }
}
