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

            document?.Save();
            document?.Close();
            
            AddTable(fileName, new String[,]{
                {"Texas", "TX"},
                {"California", "CA"},
                {"New York", "NY"},
                {"Massachusetts", "MA"}});
            
        }

        ValidateWordDocument(fileName);
        // ValidateCorruptedWordDocument(fileName);
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
                    Console.WriteLine("Id: " + error.Id);
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
    
    public static void AddTable(string fileName, string[,] data)
    {
        using (var document = WordprocessingDocument.Open(fileName, true))
        {

            var doc = document.MainDocumentPart.Document;

            Table table = new Table();

            TableProperties props = new TableProperties(
                new TableBorders(
                new TopBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 12
                },
                new BottomBorder
                {
                  Val = new EnumValue<BorderValues>(BorderValues.Single),
                  Size = 12
                },
                new LeftBorder
                {
                  Val = new EnumValue<BorderValues>(BorderValues.Single),
                  Size = 12
                },
                new RightBorder
                {
                  Val = new EnumValue<BorderValues>(BorderValues.Single),
                  Size = 12
                },
                new InsideHorizontalBorder
                {
                  Val = new EnumValue<BorderValues>(BorderValues.Single),
                  Size = 12
                },
                new InsideVerticalBorder
                {
                  Val = new EnumValue<BorderValues>(BorderValues.Single),
                  Size = 12
            }));

            table.AppendChild<TableProperties>(props);

            for (var i = 0; i <= data.GetUpperBound(0); i++)
            {
                var tr = new TableRow();
                for (var j = 0; j <= data.GetUpperBound(1); j++)
                {
                    var tc = new TableCell();
                    tc.Append(new Paragraph(new Run(new Text(data[i, j]))));
                    
                    // Assume you want columns that are automatically sized.
                    tc.Append(new TableCellProperties(
                        new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    
                    tr.Append(tc);
                }
                table.Append(tr);
            }
            doc.Body.Append(table);
            doc.Save();
        }
    }
}