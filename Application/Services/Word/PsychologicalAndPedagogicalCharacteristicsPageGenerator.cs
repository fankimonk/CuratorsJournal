using Application.Utils;
using DataAccess.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.Pages;
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

        public async Task Generate(Page? page = null)
        {
            var pages = await _pagesRepository.GetJournalPagesByTypeAsync(_journalId, PageTypes.PsychologicalAndPedagogicalCharacteristics);
            if (pages == null) throw new ArgumentException(nameof(pages));
            if (page != null)
            {
                if (!pages.Any(p => p.Id == page.Id)) throw new ArgumentException(nameof(page));
                if (page.PsychologicalAndPedagogicalCharacteristics == null) throw new ArgumentException(nameof(page));

                AppendTitle();
                AppendContent(page.PsychologicalAndPedagogicalCharacteristics);
            }
            else
            {
                foreach (var p in pages)
                {
                    if (p.PsychologicalAndPedagogicalCharacteristics == null) throw new ArgumentException(nameof(p));

                    AppendTitle();
                    AppendContent(p.PsychologicalAndPedagogicalCharacteristics);

                    if (p != pages.Last()) WordUtils.AppendPageBreak(_documentBody);
                }
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
            int maxCharactersCount = 28 * 65;

            var contentStr = characteristics.Content == null ? "" : characteristics.Content;
            if (contentStr.Length > maxCharactersCount) contentStr = contentStr.Substring(0, maxCharactersCount);

            var contentParagraph = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both })
            );

            var contentRun = new Run(WordUtils.GetRunProperties(underline: true),
                new Text(characteristics.Content == null ? "" : characteristics.Content));

            int tabCount = 28 * 13 - (Math.Max(0, contentStr.Length - 1) / 5);
            for (int i = 0; i < tabCount; i++)
                contentRun.Append(new TabChar());

            contentParagraph.Append(contentRun);

            var signatureParagraphProperties = new ParagraphProperties();
            signatureParagraphProperties.Append(new Justification { Val = JustificationValues.Both });
            Tabs tabs = new Tabs();
            tabs.Append(new TabStop()
            {
                Val = TabStopValues.Left,
                Position = 360
            });
            signatureParagraphProperties.Append(tabs);

            string workerStr = characteristics.Worker == null ? "" : GetWorkerString(characteristics.Worker);
            var worker = new Paragraph(signatureParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new TabChar(),
                    new Text("Ф. И. О., должность специалиста"))
            );
            var workerFioRun = new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(workerStr));
            int workerTabCount = 6 - Math.Max(0, workerStr.Length - 1) / 5;
            for (int i = 0; i < workerTabCount; i++)
                workerFioRun.Append(new TabChar());
            worker.Append(workerFioRun);

            var dateAndSignature = new Paragraph(signatureParagraphProperties.CloneNode(true),
                new Run(WordUtils.GetRunProperties(),
                    new TabChar(),
                    new Text("Дата, подпись"))
            );
            int dateTabCount = characteristics.Date == null ? 9 : 8;
            var dateRun = new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(characteristics.Date == null ? "" : ((DateOnly)characteristics.Date).ToString()));
            for (int i = 0; i < dateTabCount; i++)
                dateRun.Append(new TabChar());
            dateAndSignature.Append(dateRun);

            _documentBody.Append(contentParagraph, worker, dateAndSignature);
        }

        private string GetWorkerString(Worker worker)
        {
            if (worker.Position == null) return "";
            return worker.LastName + " " + worker.FirstName + " " + worker.MiddleName + ", " + worker.Position.Name;
        }
    }
}
