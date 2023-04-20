using System.Data;
using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;

namespace Epsilon.Export.Exporters;

public class CsvModuleExporter : ICanvasModuleExporter
{
    public IEnumerable<string> Formats { get; } = new[] { "CSV" };

    public string FileExtension => "csv";

    public async Task<Stream> Export(ExportData data, string format)
    {
        var stream = new MemoryStream();
        await using var writer = new StreamWriter(stream);

        using var dt = CreateDataTable(data.CourseModules);
        WriteHeader(writer, dt);
        WriteRows(writer, dt);

        await writer.FlushAsync();

        return stream;
    }

    private static DataTable CreateDataTable(IEnumerable<CourseModulePackage> data)
    {
        var dataTable = new DataTable();

        dataTable.Columns.Add("Module", typeof(string));
        dataTable.Columns.Add("KPI", typeof(string));
        dataTable.Columns.Add("Assignment", typeof(string));
        dataTable.Columns.Add("Score", typeof(string));

        foreach (var module in data)
        {
            foreach (var kpi in module.Outcomes)
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
                    if (value.Contains(';', StringComparison.InvariantCulture))
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