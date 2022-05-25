using System.Data;
using Epsilon.Abstractions.Format;
using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Format;

public class CsvFormat : ICsvFormat
{
    private readonly DataTable _dataTable = new DataTable();
    public IFileFormat FormatFile(IEnumerable<Module> modules)
    {
        _dataTable.Columns.Add("Result Id", typeof(int));
        _dataTable.Columns.Add("Assignment Id", typeof(string));
        _dataTable.Columns.Add("Assignment", typeof(string));
        _dataTable.Columns.Add("KPI", typeof(string));
        _dataTable.Columns.Add("Module", typeof(string));
        
        foreach (var module in modules)
        {
            foreach (var assignment in module.Assignments)
            {
                foreach (var outcomeResult in assignment.OutcomeResults)
                {
                    _dataTable.Rows.Add(outcomeResult?.Outcome?.Id, assignment.Id, assignment.Name, outcomeResult?.Outcome?.Title, module.Name);
                }
            }
        }
        return this;
    }

    public bool CreateDocument(string fileName)
    {
        string fileLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\{fileName}.csv";
        
        Console.WriteLine("File: " + fileLocation);
        
        ToCSV(_dataTable,fileLocation);
        
        return true;
    }
    
    private static void ToCSV(DataTable dtDataTable, string strFilePath) {  
        StreamWriter sw = new StreamWriter(strFilePath, false);  
        //headers    
        for (int i = 0; i < dtDataTable.Columns.Count; i++) {  
            sw.Write(dtDataTable.Columns[i]);  
            if (i < dtDataTable.Columns.Count - 1) {  
                sw.Write(",");  
            }  
        }  
        sw.Write(sw.NewLine);  
        foreach(DataRow dr in dtDataTable.Rows) {  
            for (int i = 0; i < dtDataTable.Columns.Count; i++) {  
                if (!Convert.IsDBNull(dr[i])) {  
                    string value = dr[i].ToString();  
                    if (value.Contains(',')) {  
                        value = String.Format("\"{0}\"", value);  
                        sw.Write(value);  
                    } else {  
                        sw.Write(dr[i].ToString());  
                    }  
                }  
                if (i < dtDataTable.Columns.Count - 1) {  
                    sw.Write(",");  
                }  
            }  
            sw.Write(sw.NewLine);  
        }  
        sw.Close();  
    } 
}