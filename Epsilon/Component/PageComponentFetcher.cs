using Epsilon.Abstractions.Component;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.Service;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace Epsilon.Component;

public class PageComponentFetcher : CompetenceComponentFetcher<Page>
{
    private readonly IPageHttpService _pageHttpService;
    private readonly IFileHttpService _fileHttpService;
    private readonly CanvasSettings _canvasSettings;
    // private readonly StudentSettings _studentSettings;

    public PageComponentFetcher(
        IPageHttpService pageHttpService,
        IFileHttpService fileHttpService,
        IOptions<CanvasSettings> canvasSettings
        // IOptions<StudentSettings> studentSettings
        )
    {
        _pageHttpService = pageHttpService;
        _fileHttpService = fileHttpService;
        _canvasSettings = canvasSettings.Value;
        // _studentSettings = studentSettings.Value;
    }

    public override async Task<Page> Fetch(string componentName, DateTime startDate, DateTime endDate)
    {
        var courseId = _canvasSettings.CourseId;
        var htmlString = await _pageHttpService.GetPageByName(courseId, componentName);

        var updatedPersonaHtml = await GetHtmlDocument(htmlString);

        var page = new Page(updatedPersonaHtml.Text);

        return page;
    }


    private async Task<HtmlDocument> GetHtmlDocument(string htmlString)
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