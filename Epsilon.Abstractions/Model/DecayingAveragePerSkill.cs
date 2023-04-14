namespace Epsilon.Abstractions.Model;

public record DecayingAveragePerSkill(
    int Skill,
    double DecayingAverage
);