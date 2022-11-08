using System.Runtime.Serialization;

namespace Epsilon.Export.Exceptions;

[Serializable]
public class NoExportersFoundException : Exception
{
    public NoExportersFoundException()
    {
    }

    protected NoExportersFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public NoExportersFoundException(IEnumerable<string> formats)
        : base($"No exporters could be found with the given formats {string.Join(",", formats)}")
    {
    }
}