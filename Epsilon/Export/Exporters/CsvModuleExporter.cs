using System.Data;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Data;
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

    public void Export(IEnumerable<Module> data, string format)
    {
        var dt = CreateDataTable(data);

        var stream = new StreamWriter($"{_options.FormattedOutputName}.{format}", false);
        WriteHeader(stream, dt);
        WriteRows(stream, dt);

        stream.Close();
    }

    private static DataTable CreateDataTable(IEnumerable<Module> modules)
    {
        var dt = new DataTable();

        dt.Columns.Add("Result Id", typeof(int));
        dt.Columns.Add("Assignment Id", typeof(string));
        dt.Columns.Add("Assignment", typeof(string));
        dt.Columns.Add("KPI", typeof(string));
        dt.Columns.Add("Module", typeof(string));

        foreach (var module in modules)
        {
            foreach (var (assessment, assignment) in module.Submissions)
            {
                foreach (var rating in assessment.Ratings)
                {
                    if (rating.Outcome != null)
                    {
                        dt.Rows.Add(rating.Outcome?.Id, assignment.Id, assignment.Name, rating.Outcome?.Title, module.Name);
                    }
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

    // TODO: Fix code smell, cognitive complexity in if statement nesting
    private static void WriteRows(TextWriter writer, DataTable dt)
    {
        foreach (DataRow dr in dt.Rows)
        {
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    var value = dr[i].ToString();
                    if (value != null)
                    {
                        if (value.Contains(';'))
                        {
                            value = $"\"{value}\"";
                            writer.Write(value);
                        }
                        else
                        {
                            writer.Write(dr[i].ToString());
                        }
                    }
                }

                if (i < dt.Columns.Count - 1)
                {
                    writer.Write(";");
                }
            }

            writer.Write(writer.NewLine);
        }
    }
}