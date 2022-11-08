using System.Data;
using System.Diagnostics;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Model;
using Microsoft.Extensions.Options;

namespace Epsilon.Export.Exporters;

public class CsvModuleExporter : ICanvasModuleExporter
{
    private readonly ExportOptions _options;

    public CsvModuleExporter(IOptions<ExportOptions> options)
    {
        _options = options.Value;
    }

    public IEnumerable<string> Formats { get; } = new[] { "csv" };

    public async Task Export(IAsyncEnumerable<ModuleOutcomeResultCollection> data, string format)
    {
        var dt = await CreateDataTable(data);

        var stream = new StreamWriter($"{_options.FormattedOutputName}.{format}", false);
        WriteHeader(stream, dt);
        WriteRows(stream, dt);

        stream.Close();
    }

    private static async Task<DataTable> CreateDataTable(IAsyncEnumerable<ModuleOutcomeResultCollection> items)
    {
        var dt = new DataTable();

        dt.Columns.Add("Result Id", typeof(int));
        dt.Columns.Add("Assignment Id", typeof(string));
        dt.Columns.Add("Assignment", typeof(string));
        dt.Columns.Add("KPI", typeof(string));
        dt.Columns.Add("Score", typeof(string));
        dt.Columns.Add("Module", typeof(string));

        await foreach (var item in items)
        {
            var links = item.Collection.Links;

            Debug.Assert(links?.OutcomesDictionary != null, "links?.OutcomesDictionary != null");
            Debug.Assert(links.AlignmentsDictionary != null, "links.AlignmentsDictionary != null");
            
            foreach (var result in item.Collection.OutcomeResults)
            {
                Debug.Assert(result.Link != null, "result.Link != null");
                var outcome = links.OutcomesDictionary[result.Link.Outcome!];
                var alignment = links.AlignmentsDictionary[result.Link.Alignment!];
                var grade = result.Grade();

                if (grade != null)
                {
                    dt.Rows.Add(
                        outcome.Id,
                        alignment.Id,
                        alignment.Name,
                        outcome.Title,
                        result.Grade(),
                        item.Module.Name
                    );
                }
            }
        }

        return dt;
    }

    private static void WriteHeader(TextWriter writer, DataTable dt)
    {
        for (var i = 0; i < dt.Columns.Count; i++)
        {
            writer.Write(dt.Columns[i]);
            if (i < dt.Columns.Count - 1)
            {
                writer.Write(";");
            }
        }

        writer.Write(writer.NewLine);
    }

    private static void WriteRows(TextWriter writer, DataTable dt)
    {
        foreach (DataRow dr in dt.Rows)
        {
            foreach (DataColumn dtColumn in dt.Columns)
            {
                var value = dr[dtColumn.Ordinal].ToString();
                if (value != null)
                {
                    if (value.Contains(';'))
                    {
                        value = $"\"{value}\"";
                        writer.Write(value);
                    }
                    else
                    {
                        writer.Write(value);
                    }
                }

                if (dtColumn.Ordinal < dt.Columns.Count - 1)
                {
                    writer.Write(";");
                }
            }

            writer.Write(writer.NewLine);
        }
    }
}