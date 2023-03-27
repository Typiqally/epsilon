namespace Epsilon.Host.WebApi.Models;

public class KpiMatrix : DocumentComponent<KpiMatrixConfig, KpiMatrixData>
{
    public override int Id { get; set; }
    public override string Type { get; set; }
    public override string Title { get; set; }
    public override string Description { get; set; }
    public override KpiMatrixConfig Config { get; set; }
    public override KpiMatrixData Data { get; set; }
}

public class KpiMatrixConfig
{
    public string[] columns { get; set; }
    public string[] rows { get; set; }
}
    
public class KpiMatrixData
{
    public string[] colors { get; set; }
}