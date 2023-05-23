namespace Epsilon.Abstractions.Service;

public interface ICompetenceDocumentService
{
    Task<Stream> WriteDocument(Stream stream, DateTime? startDate = null, DateTime? endDate = null);
}