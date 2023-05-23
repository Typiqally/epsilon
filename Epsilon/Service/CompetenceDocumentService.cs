using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Service;

namespace Epsilon.Service;

public class CompetenceDocumentService : ICompetenceDocumentService
{
    private readonly ICompetenceComponentService _competenceComponentService;

    public CompetenceDocumentService(ICompetenceComponentService competenceComponentService)
    {
        _competenceComponentService = competenceComponentService;
    }

    public async Task<Stream> WriteDocument(Stream stream, DateTime? startDate = null, DateTime? endDate = null)
    {
        var startPosition = stream.Position;

        var components = await _competenceComponentService.GetComponents<ICompetenceWordComponent>(startDate, endDate).ToListAsync();
        using var document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        document.AddMainDocumentPart();
        document.MainDocumentPart!.Document = new Document();

        foreach (var competenceWordComponent in components)
        {
            competenceWordComponent.AddToWordDocument(document.MainDocumentPart);
        }

        document.Save();
        document.Close();

        // Reset stream position to start position
        stream.Position = startPosition;

        return stream;
    }
}