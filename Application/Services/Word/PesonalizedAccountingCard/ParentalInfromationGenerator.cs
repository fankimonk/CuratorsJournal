using Application.Utils;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Application.Services.Word.PesonalizedAccountingCard
{
    public class ParentalInfromationGenerator
    {
        private readonly List<ParentalInformationRecord> _parentalInformation;

        private readonly Body _documentBody;

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
            var content = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both }));

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

                content.Append(new Run(WordUtils.GetRunProperties(underline: true, fontSize: "24"),
                    new Text(text), new TabChar()));

                if (record != _parentalInformation.Last()) content.Append(new Run(WordUtils.GetRunProperties(underline: true, fontSize: "24"), new Break()));
            }

            _documentBody.Append(content);
        }
    }
}
