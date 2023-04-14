namespace Epsilon.Abstractions.Model;

public record DecayingAveragePerLayer(
    int ArchitectureLayer,
    IEnumerable<DecayingAveragePerActivity> LayerActivities
);