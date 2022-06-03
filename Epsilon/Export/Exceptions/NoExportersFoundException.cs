namespace Epsilon.Export.Exceptions;

[Serializable]
public class NoExportersFoundException : Exception
{
    public NoExportersFoundException()
    {
    }

    public NoExportersFoundException(IEnumerable<string> formats) : base($"No exporters could be found with the given formats {string.Join(",", formats)}")
    {
    }
}