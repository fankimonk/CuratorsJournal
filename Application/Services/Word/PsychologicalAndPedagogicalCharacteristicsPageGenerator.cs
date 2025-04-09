using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class PsychologicalAndPedagogicalCharacteristicsPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public PsychologicalAndPedagogicalCharacteristicsPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.PsychologicalAndPedagogicalCharacteristics);
            if (pages == null) throw new ArgumentException(nameof(pages));

            foreach (var page in pages)
            {
                if (page.PsychologicalAndPedagogicalCharacteristics == null) throw new ArgumentException(nameof(page));

                AppendTitle();

                AppendContent(page.PsychologicalAndPedagogicalCharacteristics);

                if (page != pages.Last()) WordUtils.AppendPageBreak(_documentBody);
            }
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("ПСИХОЛОГО-ПЕДАГОГИЧЕСКАЯ"),
                    new Break()),
                new Run(WordUtils.GetRunProperties(bold:true),
                    new Text("ХАРАКТЕРИСТИКА УЧЕБНОЙ ГРУППЫ"))
            );

            _documentBody.Append(title);
        }

        private void AppendContent(PsychologicalAndPedagogicalCharacteristics characteristics)
        {
            var content = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both }),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.Content == null ? "" : characteristics.Content),
                    new TabChar())
            );

            var worker = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both }),
                new Run(WordUtils.GetRunProperties(),
                    new Text("Ф. И. О., должность специалиста")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.Worker == null ? "" : GetWorkerString(characteristics.Worker)),
                    new TabChar())
            );

            var dateAndSignature = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both }),
                new Run(WordUtils.GetRunProperties(),
                    new Text("Дата, подпись")),
                new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.Date == null ? "" : ((DateOnly)characteristics.Date).ToString()),
                    new TabChar(), new TabChar())
            );

            _documentBody.Append(content, worker, dateAndSignature);
        }

        private string GetWorkerString(Worker worker)
        {
            if (worker.Position == null) return "";
            return worker.LastName + " " + worker.FirstName + " " + worker.MiddleName + ", " + worker.Position.Name;
        }
    }
}
