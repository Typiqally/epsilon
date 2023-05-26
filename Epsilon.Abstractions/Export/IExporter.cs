﻿namespace Epsilon.Abstractions.Export;

public interface IExporter<in T>
{
    public IEnumerable<string> Formats { get; }

    void Export(T data, string format);
}