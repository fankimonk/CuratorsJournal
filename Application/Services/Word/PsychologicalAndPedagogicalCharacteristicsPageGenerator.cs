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

        private readonly int _journalId;

        private readonly int _maxLineChars = 68;
        private readonly int _linesCount = 28;

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
                AppendWorkerAndSignature(page.PsychologicalAndPedagogicalCharacteristics);
            }
            else
            {
                foreach (var p in pages)
                {
                    if (p.PsychologicalAndPedagogicalCharacteristics == null) throw new ArgumentException(nameof(p));

                    AppendTitle();
                    AppendContent(p.PsychologicalAndPedagogicalCharacteristics);
                    AppendWorkerAndSignature(p.PsychologicalAndPedagogicalCharacteristics);

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
            int maxCharactersCount = _linesCount * _maxLineChars;

            var contentStr = characteristics.Content == null ? "" : characteristics.Content;
            if (contentStr.Length > maxCharactersCount) contentStr = contentStr.Substring(0, maxCharactersCount);

            var contentParagraph = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Both })
            );

            var contentRun = new Run(WordUtils.GetRunProperties(underline: true));
                //new Text(characteristics.Content == null ? "" : characteristics.Content));

            //int tabCount = 28 * 13 - (Math.Max(0, contentStr.Length - 1) / 5);
            //for (int i = 0; i < tabCount; i++)
            //    contentRun.Append(new TabChar());

            contentParagraph.Append(contentRun);

            if (contentStr != "")
            {
                var split = contentStr.Split(' ');
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

                int linesCount = Math.Min(_linesCount, lines.Count);
                for (int i = 0; i < linesCount; i++)
                {
                    var lineStr = lines[i];

                    int tabCount = 13 - (Math.Max(0, lineStr.Length - 3) / 5);

                    var newParagraph = contentParagraph.CloneNode(true);
                    var newValueRun = contentRun.CloneNode(true);
                    newValueRun.Append(new Text(lineStr));

                    for (int j = 0; j < tabCount; j++)
                    {
                        newValueRun.Append(new TabChar());
                    }

                    newParagraph.Append(newValueRun);
                    _documentBody.Append(newParagraph);
                }

                AppendEmptyLines(_linesCount - linesCount);
            }
            else
            {
                AppendEmptyLines(_linesCount);
            }

            //_documentBody.Append(contentParagraph);
        }

        private void AppendEmptyLines(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var emptyParagraph = new Paragraph(new ParagraphProperties(
                new Justification { Val = JustificationValues.Both }
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

        private void AppendWorkerAndSignature(PsychologicalAndPedagogicalCharacteristics characteristics)
        {
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
                    new TabChar(), new TabChar(),
                    new Text(characteristics.Date == null ? "" : ((DateOnly)characteristics.Date).ToString()));
            for (int i = 0; i < dateTabCount; i++)
                dateRun.Append(new TabChar());
            dateAndSignature.Append(dateRun);

            _documentBody.Append(worker, dateAndSignature);
        }

        private string GetWorkerString(Worker worker)
        {
            if (worker.Position == null) return "";
            return worker.LastName + " " + worker.FirstName + " " + worker.MiddleName + ", " + worker.Position.Name;
        }
    }
}
