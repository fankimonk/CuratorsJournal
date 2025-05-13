using Application.Utils;
using DocumentFormat.OpenXml.AdditionalCharacteristics;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Application.Services.Word.PesonalizedAccountingCard
{
    public class ParentalInfromationGenerator
    {
        private readonly List<ParentalInformationRecord> _parentalInformation;

        private readonly Body _documentBody;

        private readonly int _maxLineChars = 86;
        private readonly int _maxLines = 7;
        private int _totalLines = 0;

        public ParentalInfromationGenerator(List<ParentalInformationRecord> parentalInformation, Body body)
        {
            _parentalInformation = parentalInformation;
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
                    new Text("Сведения о родителях и/или других родственниках, законных представителях"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(fontSize: "20"),
                    new Text("(Ф.И.О. (полностью); место жительства и/или место пребывания; место работы, занимаемая должность,"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(fontSize: "20"),
                    new Text("Телефон (дом./раоч./моб.); другие сведения)"))
            );

            _documentBody.Append(title);
        }

        private void AppendContent()
        {
            foreach (var record in _parentalInformation)
            {
                string text = "";

                string fio = "";
                fio += record.LastName == null ? "" : record.LastName + " ";
                fio += record.FirstName == null ? "" : record.FirstName + " ";
                fio += record.MiddleName == null ? "" : record.MiddleName + " ";
                text += fio == "" ? "" : fio + "; ";

                text += record.PlaceOfResidence == null ? "" : record.PlaceOfResidence + "; ";
                text += record.PlaceOfWork == null ? "" : record.PlaceOfWork + ", ";
                text += record.Position == null ? "" : record.Position + ", ";
                text += record.HomePhoneNumber == null ? "" : record.HomePhoneNumber + ", ";
                text += record.WorkPhoneNumber == null ? "" : record.WorkPhoneNumber + ", ";
                text += record.MobilePhoneNumber == null ? "" : record.MobilePhoneNumber + "; ";
                text += record.OtherInformation == null ? "" : record.OtherInformation + ";";

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
