using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using HtmlAgilityPack;

namespace Epsilon.Export.Exporters;

public class WordModuleExporter : ICanvasModuleExporter
{
    private static readonly TableBorders s_defaultBorders = new(
        new TopBorder {Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3},
        new BottomBorder {Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3},
        new LeftBorder {Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3},
        new RightBorder {Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3},
        new InsideHorizontalBorder {Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6},
        new InsideVerticalBorder {Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6}
    );

    private static readonly TableProperties s_defaultTableProperties = new(s_defaultBorders);

    private static readonly TableRow s_defaultHeader = new(
        CreateTextCell("KPI's"),
        CreateTextCell("Assignments"),
        CreateTextCell("Score")
    );

    private readonly IFileHttpService _fileService;
    public IEnumerable<string> Formats { get; } = new[] {"word", "docx"};
    public string FileExtension => "docx";

    public WordModuleExporter(IFileHttpService fileService)
    {
        _fileService = fileService;
    }

    public WordModuleExporter()
    {
        throw new NotImplementedException();
    }

    public async Task<Stream> Export(ExportData data, string format)
    {
        var stream = new MemoryStream();
        using var document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        document.AddMainDocumentPart();
        document.MainDocumentPart!.Document = new Document(new Body());

        var body = document.MainDocumentPart.Document.Body;
        var cellValueBuilder = new StringBuilder();
        var cellValueOutComeResultsBuilder = new StringBuilder();

        var altChunkId = "HomePage";

        var personaHtml = new HtmlDocument();
        personaHtml.LoadHtml(data.PersonaHtml);

        var updatedPersonaHtml = await ReplaceImageSrcWithBase64String(personaHtml);

        using var ms = new MemoryStream(new UTF8Encoding(true).GetPreamble()
            .Concat(Encoding.UTF8.GetBytes($"<html>{updatedPersonaHtml.Text}</html>")).ToArray());

        var formatImportPart =
            document.MainDocumentPart.AddAlternativeFormatImportPart(
                AlternativeFormatImportPartType.Html, altChunkId);

        formatImportPart.FeedData(ms);
        AltChunk altChunk = new AltChunk();
        altChunk.Id = altChunkId;

        body?.Append(altChunk);
        ms.DisposeAsync();
        body?.Append(new Paragraph(new Run(new Break() {Type = BreakValues.Page})));

        foreach (var module in data.CourseModules)
        {
            body?.Append(CreateText(module.Name));

            var table = new Table();

            table.AppendChild(s_defaultTableProperties.CloneNode(true));
            table.Append(s_defaultHeader.CloneNode(true));

            var rows = module.Outcomes.Select(kpi =>
            {
                foreach (var assignment in kpi.Assignments)
                {
                    cellValueBuilder.AppendLine($"{assignment.Name} {assignment.Url}");
                    cellValueOutComeResultsBuilder.AppendLine(assignment.Score);
                }

                var row = new TableRow(
                    CreateTextCell($"{kpi.Name} {kpi.Description}"),
                    CreateTextCell(cellValueBuilder.ToString()),
                    CreateTextCell(cellValueOutComeResultsBuilder.ToString())
                );

                cellValueBuilder.Clear();
                cellValueOutComeResultsBuilder.Clear();

                return row;
            });

            table.Append(rows);

            body?.Append(table);
        }

        document.Save();
        document.Close();

        return stream;
    }

    private static Paragraph CreateText(string text) => new(new Run(new Text(text)));

    private static TableCell CreateTextCell(string text) => new(
        CreateText(text),
        new TableCellProperties(new TableCellWidth {Type = TableWidthUnitValues.Auto})
    );

    private async Task<HtmlDocument> ReplaceImageSrcWithBase64String(HtmlDocument htmlDoc)
    {
        if (htmlDoc.DocumentNode.SelectNodes("//img") == null)
            return null;
        foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
        {
            var imageSrc = node
                .SelectNodes("//img")
                .First()
                .Attributes["src"].Value;

            if (imageSrc == null)
                throw new ArgumentNullException(nameof(imageSrc));

            var imageBytes = await _fileService.GetFileByteArray(imageSrc);
            var imageBase64 = Convert.ToBase64String(imageBytes.ToArray());

            node.SetAttributeValue("src", $"data:image/jpeg;base64,{imageBase64}");
        }

        return htmlDoc;
    }
}