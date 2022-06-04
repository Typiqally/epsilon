using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Data;
using ExcelLibrary.SpreadSheet;

namespace Epsilon.Export;

public class ExcelCanvasModuleFileExporter : ICanvasModuleFileExporter
{
    public bool CanExport(string format) => format.ToLower() == "xls";

    public  List<Outcome> GetAllOutcomesTypes(Module module)
    {
        List<Outcome> addedOutcomes = new List<Outcome>();
        if (module.HasAssignments())
        {
            foreach (var assignment in module.Assignments)
            {
                foreach (var result in assignment.OutcomeResults)
                {
                    if (result.Outcome != null && !addedOutcomes.Contains(result.Outcome))
                    {
                        addedOutcomes.Add(result.Outcome);
                    }
                }
            }
        }

        return addedOutcomes.OrderByDescending(o => o.Title.Length).ToList();
    }

    public int GetOutcomeRow(List<Outcome> outcomes, Outcome outcome)
    {
        var result = outcomes.Find(o => o.Title == outcome.Title);
        if (result != null)
        {
            return outcomes.IndexOf(result);
        }

        return 0;
    }

    public void Export(IEnumerable<Module> modules, string path)
    {
        Workbook workbook = new Workbook();
        
        foreach (var module in modules)
        {
            if (module.HasAssignments())
            {
                List<Outcome> outcomes = GetAllOutcomesTypes(module);
                Worksheet worksheet = new Worksheet(module.Name);
                //Because reasons @source https://stackoverflow.com/a/8127642 
                for(int i = 0;i < 100; i++)
                    worksheet.Cells[i,0] = new Cell("");

                //Adding all the outcomes. 
                for (int index = 0; index < outcomes.Count; index++)
                {
                    worksheet.Cells[index, 0] = new Cell(outcomes[index].Title);
                }

                foreach (var assignment in module.Assignments)
                {
                    foreach (var outcomeResult in assignment.OutcomeResults)
                    {
                        if (outcomeResult.Outcome != null)
                        {
                            int row = GetOutcomeRow(outcomes, outcomeResult.Outcome);
                            
                            //Adding assignments to the outcomes 
                            string cellValue = worksheet.Cells[row, 1].StringValue;
                            cellValue += (cellValue != "" ? "\n": "") +  assignment.Name + " " + assignment.Url  ;
                            
                            worksheet.Cells[row, 1] = new Cell(cellValue);
                        }

                    }
                }

                worksheet.Cells.ColumnWidth[0, 0] = 5000;
                worksheet.Cells.ColumnWidth[0, 1] = 8000;
                workbook.Worksheets.Add(worksheet); 
                
            }
        }
        workbook.Save(path + ".xls");
    }
}