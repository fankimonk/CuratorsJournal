using Application.Utils;
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
            for (int i = 0; i < groupActives.Count; i++)
            {
                var content = new Paragraph(
                    new ParagraphProperties(
                        new Justification { Val = JustificationValues.Both })
                );

                var positionStr = groupActives[i].PositionName ?? "";
                if (positionStr.Length > 25) positionStr = positionStr.Substring(0, 25);
                int positionTabCount = Math.Max(0, positionStr.Length - 1) / 5 + 1;
                Run positionRun = new();
                if (i <= 2) positionRun.Append(WordUtils.GetRunProperties());
                else positionRun.Append(WordUtils.GetRunProperties(underline: true));
                positionRun.Append(new Text(positionStr));

                var studentStr = groupActives[i].Student == null ? "" : GetStudentFIO(groupActives[i].Student);
                if (studentStr.Length > 65 - positionStr.Length) studentStr = studentStr.Substring(0, 65 - positionStr.Length);
                int tabsLeftCount = 13 - positionTabCount - Math.Max(0, studentStr.Length - 1) / 5;
                Run studentRun = new Run(WordUtils.GetRunProperties(underline: true),
                    new TabChar(),
                    new Text(studentStr));
                for (int j = 0; j < tabsLeftCount; j++)
                    studentRun.Append(new TabChar());

                content.Append(positionRun, studentRun);
                _documentBody.Append(content);
            }
        }

        private string GetStudentFIO(Student student)
        {
            return student.LastName + " " + student.FirstName + " " + student.MiddleName;
        }
    }
}
