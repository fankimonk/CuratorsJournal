using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

string filePath = "example.docx";

using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
{
    // Создаём главный раздел документа
    var mainPart = wordDoc.AddMainDocumentPart();
    mainPart.Document = new Document();

    var body = new Body();

    // Книжная ориентация
    SectionProperties portraitSection = new SectionProperties(
        new PageSize() { Width = 11906, Height = 16838, Orient = PageOrientationValues.Portrait }); // Книжная ориентация

    // Альбомная ориентация
    SectionProperties landscapeSection = new SectionProperties(
        new PageSize() { Width = 16838, Height = 11906, Orient = PageOrientationValues.Landscape }); // Альбомная ориентация

    // Первая секция - книжная ориентация
    var paragraph1 = new Paragraph(new Run(new Text("Книжная ориентация")));
    var sectionBreakPortrait = new Paragraph(
        new ParagraphProperties(new SectionProperties(portraitSection.CloneNode(true) as SectionProperties)));

    body.Append(paragraph1);
    body.Append(sectionBreakPortrait);

    // Вторая секция - альбомная ориентация
    var paragraph2 = new Paragraph(new Run(new Text("Альбомная ориентация")));
    var sectionBreakLandscape = new Paragraph(
        new ParagraphProperties(new SectionProperties(landscapeSection.CloneNode(true) as SectionProperties)));

    body.Append(paragraph2);
    body.Append(sectionBreakLandscape);

    // Третья секция - снова книжная ориентация
    var paragraph3 = new Paragraph(new Run(new Text("Снова книжная ориентация")));
    //var sectionBreakPortrait2 = new Paragraph(
    //    new ParagraphProperties(new SectionProperties(portraitSection.CloneNode(true) as SectionProperties)));

    body.Append(paragraph3);
    body.Append(sectionBreakPortrait.CloneNode(true));

    // Добавляем тело документа
    mainPart.Document.Append(body);
    mainPart.Document.Save();
}

Console.WriteLine("Документ успешно создан.");
