namespace Epsilon.Abstractions.Component;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CompetenceComponentNameAttribute : Attribute
{
    public CompetenceComponentNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}