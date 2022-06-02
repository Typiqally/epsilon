using System.Data;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Export;

public class CsvCanvasModuleFileExporter : ICanvasModuleFileExporter
{
    public bool CanExport(string format) => format == "csv";

    public void Export(IEnumerable<Module> data, string path)
    {
        var dt = CreateDataTable(data);

        var stream = new StreamWriter(path + ".csv", false);
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
            foreach (var assignment in module.Assignments)
            {
                foreach (var outcomeResult in assignment.OutcomeResults)
                {
                    dt.Rows.Add(outcomeResult.Outcome?.Id, assignment.Id, assignment.Name, outcomeResult?.Outcome?.Title, module.Name);
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
                writer.Write(",");
            }
        }

        writer.Write(writer.NewLine);
    }

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
                        if (value.Contains(','))
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
                    writer.Write(",");
                }
            }

            writer.Write(writer.NewLine);
        }
    }
}