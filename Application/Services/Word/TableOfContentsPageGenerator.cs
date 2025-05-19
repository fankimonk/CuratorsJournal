using Application.Utils;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Application.Services.Word
{
    public class TableOfContentsPageGenerator(Body body)
    {
        private readonly Body _documentBody = body;

        public void Generate()
        {
            AppendTitle();
            AppendContent();
            WordUtils.AppendPageBreak(_documentBody);
        }

        private void AppendTitle()
        {
            var titleParagraphProperties = new ParagraphProperties(
                new Justification { Val = JustificationValues.Center },
                new SpacingBetweenLines { After = "250" });
            var titleParagraph = new Paragraph(titleParagraphProperties);

            var titleRun = new Run(WordUtils.GetRunProperties(bold: true),
                new Text("СОДЕРЖАНИЕ"));

            titleParagraph.Append(titleRun);
            _documentBody.Append(titleParagraph);
        }

        private void AppendContent()
        {
            Tabs tabs = new Tabs();
            tabs.Append(new TabStop()
            {
                Val = TabStopValues.Right,
                Position = 9355,
                Leader = TabStopLeaderCharValues.Dot
            });

            var paragraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Both }, tabs);

            var journalKeepingParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var journalKeepingRun = new Run(WordUtils.GetRunProperties(),
                new Text("Ведение журнала"),
                new TabChar());
            journalKeepingParagraph.Append(journalKeepingRun);
            _documentBody.Append(journalKeepingParagraph);

            var contactPhonesParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var contactPhonesRun = new Run(WordUtils.GetRunProperties(),
                new Text("Контактные телефоны"),
                new TabChar());
            contactPhonesParagraph.Append(contactPhonesRun);
            _documentBody.Append(contactPhonesParagraph);

            var holidaysParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var holidaysRun = new Run(WordUtils.GetRunProperties(),
                new Text("Календарь праздников"),
                new TabChar());
            holidaysParagraph.Append(holidaysRun);
            _documentBody.Append(holidaysParagraph);

            var spcParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var spcRun = new Run(WordUtils.GetRunProperties(),
                new Text("Социальная характеристика учебной группы"),
                new TabChar());
            spcParagraph.Append(spcRun);
            _documentBody.Append(spcParagraph);

            var epsParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var epsRun = new Run(WordUtils.GetRunProperties(),
                new Text("График учебного процесса"),
                new TabChar());
            epsParagraph.Append(epsRun);
            _documentBody.Append(epsParagraph);

            var dokiParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var dokiRun = new Run(WordUtils.GetRunProperties(),
                new Text("Динамика основных показателей группы за период обучения"),
                new TabChar());
            dokiParagraph.Append(dokiRun);
            _documentBody.Append(dokiParagraph);

            var groupActivesParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var groupActivesRun = new Run(WordUtils.GetRunProperties(),
                new Text("Актив учебной группы"),
                new TabChar());
            groupActivesParagraph.Append(groupActivesRun);
            _documentBody.Append(groupActivesParagraph);

            var studentListParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var studentListRun = new Run(WordUtils.GetRunProperties(),
                new Text("Список студентов"),
                new TabChar());
            studentListParagraph.Append(studentListRun);
            _documentBody.Append(studentListParagraph);

            var pacParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var pacRun = new Run(WordUtils.GetRunProperties(),
                new Text("Карты персонифицированного учета"),
                new TabChar());
            pacParagraph.Append(pacRun);
            _documentBody.Append(pacParagraph);

            var shcParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var shcRun = new Run(WordUtils.GetRunProperties(),
                new Text("Карты здоровья студентов"),
                new TabChar());
            shcParagraph.Append(shcRun);
            _documentBody.Append(shcParagraph);

            var fpaParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var fpaRun = new Run(WordUtils.GetRunProperties(),
                new Text("Учет итоговой успеваемости студентов группы"),
                new TabChar());
            fpaParagraph.Append(fpaRun);
            _documentBody.Append(fpaParagraph);

            var iaewaParagraph1 = new Paragraph();
            var iaewaRun1 = new Run(WordUtils.GetRunProperties(),
                new Text("Учет идеологической и воспитательной работы"));
            var iaewaParagraph2 = new Paragraph(paragraphProperties.CloneNode(true));
            var iaewaRun2 = new Run(WordUtils.GetRunProperties(),
                new Text("куратора учебной группы (по месяцам)"),
                new TabChar());
            iaewaParagraph1.Append(iaewaRun1);
            iaewaParagraph2.Append(iaewaRun2);
            _documentBody.Append(iaewaParagraph1, iaewaParagraph2);

            var ihaParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var ihaRun = new Run(WordUtils.GetRunProperties(),
                new Text("Учет информационных часов"),
                new TabChar());
            ihaParagraph.Append(ihaRun);
            _documentBody.Append(ihaParagraph);

            var cpParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var cpRun = new Run(WordUtils.GetRunProperties(),
                new Text("Участие куратора в работе педагогических семинаров"),
                new TabChar());
            cpParagraph.Append(cpRun);
            _documentBody.Append(cpParagraph);

            var literatureWorkParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var literatureWorkRun = new Run(WordUtils.GetRunProperties(),
                new Text("Работа с научно-методической и педагогической литературой"),
                new TabChar());
            literatureWorkParagraph.Append(literatureWorkRun);
            _documentBody.Append(literatureWorkParagraph);

            var ppcParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var ppcRun = new Run(WordUtils.GetRunProperties(),
                new Text("Психолого-педагогическая характеристика учебной группы"),
                new TabChar());
            ppcParagraph.Append(ppcRun);
            _documentBody.Append(ppcParagraph);

            var rarParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var rarRun = new Run(WordUtils.GetRunProperties(),
                new Text("Рекомендации и замечания лиц, проверяющих работу куратора"),
                new TabChar());
            rarParagraph.Append(rarRun);
            _documentBody.Append(rarParagraph);

            var traditionsParagraph = new Paragraph(paragraphProperties.CloneNode(true));
            var traditionsRun = new Run(WordUtils.GetRunProperties(),
                new Text("Традиции вуза, факультета, группы"),
                new TabChar());
            traditionsParagraph.Append(traditionsRun);
            _documentBody.Append(traditionsParagraph);
        }
    }
}
