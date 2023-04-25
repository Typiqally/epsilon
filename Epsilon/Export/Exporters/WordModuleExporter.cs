using System.Globalization;
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
    private readonly IFileHttpService _fileService;

    public WordModuleExporter(IFileHttpService fileService)
    {
        _fileService = fileService;
    }

    private static readonly TableBorders s_defaultBorders = new TableBorders(new TopBorder
    {
        Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,
    }, new BottomBorder
    {
        Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,
    }, new LeftBorder
    {
        Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,
    }, new RightBorder
    {
        Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,
    }, new InsideHorizontalBorder
    {
        Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6,
    }, new InsideVerticalBorder
    {
        Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6,
    });

    private static readonly TableProperties s_defaultTableProperties = new TableProperties(s_defaultBorders);

    private static readonly TableRow s_defaultHeader = new TableRow(CreateTextCell("KPI's"), CreateTextCell("Assignments"), CreateTextCell("Score"));

    public IEnumerable<string> Formats { get; } = new[]
    {
        "WORD",
        "DOCX",
    };

    public string FileExtension => "docx";

    public async Task<Stream> Export(ExportData data, string format)
    {
        var stream = new MemoryStream();
        var document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        document.AddMainDocumentPart();
        document.MainDocumentPart!.Document = new Document(new Body());

        var body = document.MainDocumentPart.Document.Body;
        var cellValueBuilder = new StringBuilder();
        var cellValueOutComeResultsBuilder = new StringBuilder();

        const string altChunkId = "HomePage";

        var personaHtml = await GetPersonaHtmlDocument(_fileService, data.PersonaHtml);

        using var personaHtmlStream = new MemoryStream(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes($"<html>{personaHtml.Text}</html>")).ToArray());
        var formatImportPart = document.MainDocumentPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkId);

        formatImportPart.FeedData(personaHtmlStream);
        var altChunk = new AltChunk
        {
            Id = altChunkId,
        };

        body?.Append(altChunk);
        body?.Append(new Paragraph(new Run(new Break
        {
            Type = BreakValues.Page,
        })));

        await personaHtmlStream.DisposeAsync();

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
                    cellValueBuilder.AppendLine(CultureInfo.InvariantCulture, $"{assignment.Name} {assignment.Url}");
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

    private static Paragraph CreateText(string text)
    {
        return new Paragraph(new Run(new Text(text)));
    }

    private static TableCell CreateTextCell(string text)
    {
        return new TableCell(CreateText(text), new TableCellProperties(new TableCellWidth
        {
            Type = TableWidthUnitValues.Auto,
        }));
    }

    private static async Task<HtmlDocument> GetPersonaHtmlDocument(IFileHttpService fileService, string htmlString)
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
                var imageBytes = await fileService.GetFileByteArray(new Uri(imageSrc));
                var imageBase64 = Convert.ToBase64String(imageBytes.ToArray());

                node.SetAttributeValue("src", $"data:image/jpeg;base64,{imageBase64}");
            }
        }

        return htmlDoc;
    }
}