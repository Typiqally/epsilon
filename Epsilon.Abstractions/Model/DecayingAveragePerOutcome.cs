namespace Epsilon.Abstractions.Model;

public record DecayingAveragePerOutcome(
    int ReferenceId,
    double DecayingAverage
);