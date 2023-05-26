using Epsilon.Abstractions.Component;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.Service;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace Epsilon.Component;

public class PersonaPageComponentFetcher : CompetenceComponentFetcher<PersonaPage>
{
    private readonly IPageHttpService _pageHttpService;
    private readonly IFileHttpService _fileHttpService;
    private readonly CanvasSettings _canvasSettings;

    public PersonaPageComponentFetcher(
        IPageHttpService pageHttpService,
        IFileHttpService fileHttpService,
        IOptions<CanvasSettings> canvasSettings
    )
    {
        _pageHttpService = pageHttpService;
        _fileHttpService = fileHttpService;
        _canvasSettings = canvasSettings.Value;
    }

    public override async Task<PersonaPage> Fetch(DateTime startDate, DateTime endDate)
    {
        var courseId = _canvasSettings.CourseId;
        var personaHtml = await _pageHttpService.GetPageByName(courseId, "front_page");

        var updatedPersonaHtml = await GetPersonaHtmlDocument(personaHtml);

        var personaPage = new PersonaPage(updatedPersonaHtml.Text);

        return personaPage;
    }

    private async Task<HtmlDocument> GetPersonaHtmlDocument(string htmlString)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlString);
        if (htmlDoc.DocumentNode.SelectNodes("//img") == null)
        {
            return htmlDoc;
        }

        foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
        {
            var imageSrc = node
                .SelectNodes("//img")
                .First()
                .Attributes["src"].Value;

            if (imageSrc != null)
            {
                var imageBytes = await _fileHttpService.GetFileByteArray(new Uri(imageSrc));
                var imageBase64 = Convert.ToBase64String(imageBytes.ToArray());

                node.SetAttributeValue("src", $"data:image/jpeg;base64,{imageBase64}");
            }
        }

        return htmlDoc;
    }

}