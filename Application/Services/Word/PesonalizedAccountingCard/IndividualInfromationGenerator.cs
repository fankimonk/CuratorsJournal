using Application.Utils;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Application.Services.Word.PesonalizedAccountingCard
{
    public class IndividualInfromationGenerator
    {
        private readonly List<IndividualInformationRecord> _individualInformation;

        private readonly Body _documentBody;

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

                content.Append(new Run(WordUtils.GetRunProperties(underline: true, fontSize: "24"),
                    new Text(text), new TabChar()));

                if (record != _individualInformation.Last()) content.Append(new Run(WordUtils.GetRunProperties(underline: true, fontSize: "24"), new Break()));
            }

            _documentBody.Append(content);
        }
    }
}
