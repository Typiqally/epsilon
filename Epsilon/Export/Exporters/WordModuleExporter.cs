using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Model;
using Microsoft.Extensions.Options;

namespace Epsilon.Export.Exporters;

public class WordModuleExporter : ICanvasModuleExporter
{
    private readonly ExportOptions _options;

    public WordModuleExporter(IOptions<ExportOptions> options)
    {
        _options = options.Value;
    }

    public IEnumerable<string> Formats { get; } = new[] { "word" };

    public async Task Export(IAsyncEnumerable<ModuleOutcomeResultCollection> data, string format)
    {
        string fileName = $"{_options.FormattedOutputName}.docx";
        using (var document = WordprocessingDocument.Create(fileName,
                   WordprocessingDocumentType.Document))
        {
            document.AddMainDocumentPart();
            document.MainDocumentPart!.Document = new Document(new Body());
            var doc = document.MainDocumentPart.Document;
            // Add a main document part. 
            // MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

            // Create the document structure and add some text.
            // mainPart.Document = new Document();
            // Body body = mainPart.Document.AppendChild(new Body());
            // Paragraph para = body.AppendChild(new Paragraph());
            // Run run = para.AppendChild(new Run());
            // run.AppendChild(new Text("Create text in body - CreateWordprocessingDocument"));


            await foreach (var item in data.Where(static m => m.Collection.OutcomeResults.Any()))
            {
                var links = item.Collection.Links;
                var alignments = links.AlignmentsDictionary;
                var outcomes = links.OutcomesDictionary;

                Table table = new Table();
                TableRow headerRow = new TableRow();
                var kpiCell = new TableCell();
                kpiCell.Append(new Paragraph(new Run(new Text("KPI"))));
                var assignmentCell = new TableCell();
                assignmentCell.Append(new Paragraph(new Run(new Text("Assignment(s)"))));
                var scoreCell = new TableCell();
                scoreCell.Append(new Paragraph(new Run(new Text("Scores"))));
                headerRow.Append(kpiCell);
                headerRow.Append(assignmentCell);
                headerRow.Append(scoreCell);
                
                table.Append(headerRow);

                // foreach (var (outcomeId, outcome) in outcomes)
                // {
                //     var assignmentIds = item.Collection.OutcomeResults
                //         .Where(o => o.Link.Outcome == outcomeId && o.Grade() != null)
                //         .Select(static o => o.Link.Assignment)
                //         .ToArray();

                // if (assignmentIds.Any())
                // {
                //     TableRow row = new TableRow();
                //     
                //     var outcomeCell = new TableCell();
                //     outcomeCell.Append(new Text($"{outcome.Title} {outcome.ShortDescription()}"));
                //     
                //     row.Append(new TableCell(outcomeCell));
                //
                //     var cellValueBuilder = new StringBuilder();
                //
                //     foreach (var (_, alignment) in alignments.Where(a => assignmentIds.Contains(a.Key)))
                //     {
                //         cellValueBuilder.AppendLine($"{alignment.Name} {alignment.Url}");
                //     }
                //
                //     var assignmentXCell = new TableCell();
                //     assignmentXCell.Append(new Text(cellValueBuilder.ToString()));
                //     
                //     row.Append(new TableCell(assignmentXCell));
                //
                //     var cellValueOutComeResultsBuilder = new StringBuilder();
                //     foreach (var outcomeResult in item.Collection.OutcomeResults.Where(result =>
                //                  result.Link.Outcome == outcomeId))
                //     {
                //         cellValueOutComeResultsBuilder.AppendLine(outcomeResult.Grade());
                //     }
                //     
                //     var scoresCell = new TableCell();
                //     scoresCell.Append(new Text(cellValueBuilder.ToString()));
                //     
                //     row.Append(scoresCell);
                // }
                // }

                doc?.Body?.Append(table);
            }

            document?.Save();
            document?.Close();
        }

        ValidateWordDocument(fileName);
        ValidateCorruptedWordDocument(fileName);
    }

    public static void ValidateWordDocument(string filepath)
    {
        using (WordprocessingDocument wordprocessingDocument =
               WordprocessingDocument.Open(filepath, true))
        {
            try
            {
                OpenXmlValidator validator = new OpenXmlValidator();
                int count = 0;
                foreach (ValidationErrorInfo error in
                         validator.Validate(wordprocessingDocument))
                {
                    count++;
                    Console.WriteLine("Error " + count);
                    Console.WriteLine("Description: " + error.Description);
                    Console.WriteLine("ErrorType: " + error.ErrorType);
                    Console.WriteLine("Node: " + error.Node);
                    Console.WriteLine("Path: " + error.Path.XPath);
                    Console.WriteLine("Part: " + error.Part.Uri);
                    Console.WriteLine("-------------------------------------------");
                }

                Console.WriteLine("count={0}", count);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            wordprocessingDocument.Close();
        }
    }

    public static void ValidateCorruptedWordDocument(string filepath)
    {
        // Insert some text into the body, this would cause Schema Error
        using (WordprocessingDocument wordprocessingDocument =
               WordprocessingDocument.Open(filepath, true))
        {
            // Insert some text into the body, this would cause Schema Error
            // Body body = wordprocessingDocument.MainDocumentPart.Document.Body;
            // Run run = new Run(new Text("some text"));
            // body.Append(run);

            try
            {
                OpenXmlValidator validator = new OpenXmlValidator();
                int count = 0;
                foreach (ValidationErrorInfo error in
                         validator.Validate(wordprocessingDocument))
                {
                    count++;
                    Console.WriteLine("Error " + count);
                    Console.WriteLine("Description: " + error.Description);
                    Console.WriteLine("ErrorType: " + error.ErrorType);
                    Console.WriteLine("Node: " + error.Node);
                    Console.WriteLine("Path: " + error.Path.XPath);
                    Console.WriteLine("Part: " + error.Part.Uri);
                    Console.WriteLine("-------------------------------------------");
                }

                Console.WriteLine("count={0}", count);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}