using System.Runtime.Serialization;

namespace Epsilon.Export.Exceptions;

[Serializable]
public class NoExportersFoundException : Exception
{
    public NoExportersFoundException()
    {
    }

    public NoExportersFoundException(IEnumerable<string> formats)
        : base($"No exporters could be found with the given formats {string.Join(",", formats)}")
    {
    }

    public NoExportersFoundException(string? message) : base(message)
    {
    }

    public NoExportersFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NoExportersFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}