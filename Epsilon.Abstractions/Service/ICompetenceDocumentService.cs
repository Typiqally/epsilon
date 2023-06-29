namespace Epsilon.Abstractions.Service;

public interface ICompetenceDocumentService
{
    Task<Stream> WriteDocument(Stream stream, string name, DateTime startDate, DateTime endDate);
}