using System.Data;
using Epsilon.Abstractions.Export;
using Microsoft.Extensions.Options;
using Epsilon.Abstractions.Model;

namespace Epsilon.Export.Exporters;

public class CsvModuleExporter : ICanvasModuleExporter
{
    private readonly ExportOptions _options;

    public CsvModuleExporter(IOptions<ExportOptions> options)
    {
        _options = options.Value;
    }

    public IEnumerable<string> Formats { get; } = new[] { "csv" };

    public async Task<Stream> Export(ExportData data, string format)
    {
        var filePath = $"{_options.FormattedOutputName}.{format}";
        var dt = CreateDataTable(data.CourseModules);
    
        var stream = new StreamWriter(filePath, false);
        WriteHeader(stream, dt);
        WriteRows(stream, dt);

        stream.Close();

        return new FileStream(filePath, FileMode.Open);
    }

    private static DataTable CreateDataTable(IEnumerable<CourseModule> data)
    {
        var dataTable = new DataTable();
        
        dataTable.Columns.Add("Module", typeof(string));
        dataTable.Columns.Add("KPI", typeof(string));
        dataTable.Columns.Add("Assignment", typeof(string));
        dataTable.Columns.Add("Score", typeof(string));

        foreach (var module in data)
        {
            foreach (var kpi in module.Kpis)
            {
                foreach (var assignment in kpi.Assignments)
                {
                    dataTable.Rows.Add(module.Name, kpi.Name, assignment.Name, assignment.Score);
                }
            }
        }

        return dataTable;
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