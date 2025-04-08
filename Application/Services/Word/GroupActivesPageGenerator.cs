using DataAccess.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;

namespace Application.Services.Word
{
    public class GroupActivesPageGenerator
    {
        private readonly IPagesRepository _pagesRepository;

        private readonly Body _documentBody;

        private int _journalId;

        public GroupActivesPageGenerator(int journalId, Body body, IPagesRepository pagesRepository)
        {
            _journalId = journalId;
            _documentBody = body;
            _pagesRepository = pagesRepository;
        }

        public async Task Generate()
        {
            var pages = await _pagesRepository.GetJournalPagesByType(_journalId, PageTypes.GroupActives);
            if (pages == null) throw new ArgumentException(nameof(pages));

            foreach (var page in pages)
            {
                if (page.GroupActives == null) throw new ArgumentException(nameof(page));

                AppendTitle();

                AppendActives(page.GroupActives);
            }

            WordUtils.AppendPageBreak(_documentBody);
        }

        private void AppendTitle()
        {
            var title = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }),
                new Run(WordUtils.GetRunProperties(bold: true),
                    new Text("АКТИВ УЧЕБНОЙ ГРУППЫ"))
            );

            _documentBody.Append(title);
        }

        private void AppendActives(List<GroupActive> groupActives)
        {
            foreach (var active in groupActives)
            {
                var content = new Paragraph(
                    new ParagraphProperties(
                        new Justification { Val = JustificationValues.Both }),
                    new Run(WordUtils.GetRunProperties(),
                        new Text(active.PositionName == null ? "" : active.PositionName)),
                    new Run(WordUtils.GetRunProperties(underline: true),
                        new TabChar(),
                        new Text(active.Student == null ? "" : GetStudentFIO(active.Student)),
                        new TabChar())
                );

                _documentBody.Append(content);
            }
        }

        private string GetStudentFIO(Student student)
        {
            return student.LastName + " " + student.FirstName + " " + student.MiddleName;
        }
    }
}
