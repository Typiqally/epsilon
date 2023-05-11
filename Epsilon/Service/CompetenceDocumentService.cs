using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Service;

namespace Epsilon.Service;

public class CompetenceDocumentService : ICompetenceDocumentService
{
    private readonly IWordDocumentBuilder _wordDocumentBuilder;
    private readonly ICompetenceComponentService _competenceComponentService;

    public CompetenceDocumentService(
        IWordDocumentBuilder wordDocumentBuilder,
        ICompetenceComponentService competenceComponentService
    )
    {
        _wordDocumentBuilder = wordDocumentBuilder;
        _competenceComponentService = competenceComponentService;
    }

    public async Task<Stream> WriteDocument(Stream stream)
    {
        var startPosition = stream.Position;

        var components = await _competenceComponentService.GetComponents<ICompetenceWordComponent>().ToListAsync();
        using var document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        document.AddMainDocumentPart();
        document.MainDocumentPart!.Document = _wordDocumentBuilder.Build(document.MainDocumentPart, components);

        document.Save();
        document.Close();

        // Reset stream position to start position
        stream.Position = startPosition;

        return stream;
    }
}