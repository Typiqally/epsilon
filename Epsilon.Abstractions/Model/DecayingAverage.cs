namespace Epsilon.Abstractions.Model;

public record DecayingAverage(
    double Score,
    int ArchitectureLayer,
    int Activity
);