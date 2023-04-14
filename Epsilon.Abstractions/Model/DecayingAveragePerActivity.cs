namespace Epsilon.Abstractions.Model;

public record DecayingAveragePerActivity(
    int Activity,
    double DecayingAverage
);