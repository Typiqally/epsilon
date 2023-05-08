import { IHboIDomain, ProfessionalTaskResult } from "/@/logic/Api"

export class DecayingAverageLogic {
    private static GetDecayingAverageForAllOutcomes(
        taskResults: ProfessionalTaskResult[],
        domain: IHboIDomain
    ): DecayingAveragePerLayer[] {
        return domain.architectureLayers?.map((l) => {
            return {
                architectureLayer: l.id,
                layerActivities: Object.entries(
                    this.groupBy(
                        taskResults.filter(
                            (layer) => layer.architectureLayer === l.id
                        ),
                        (r) => r.outcomeId
                    )
                ).map(([i, j]) => {
                    return {
                        outcome: i,
                        activity: j.at(0)?.activity,
                        decayingAverage:
                            this.GetDecayingAverageFromOneOutcomeType(j),
                    }
                }) as unknown as DecayingAveragePerActivity[],
            }
        }) as DecayingAveragePerLayer[]
    }

    static GetAverageDevelopmentScores(
        taskResults: ProfessionalTaskResult[],
        domain: IHboIDomain
    ): DecayingAveragePerLayer[] {
        const canvasDecaying = this.GetDecayingAverageForAllOutcomes(
            taskResults,
            domain
        )
        return domain.architectureLayers?.map((layer) => {
            return {
                architectureLayer: layer.id,
                layerActivities: domain.activities?.map((activity) => {
                    let totalScoreActivity = 0
                    let totalScoreArchitectureActivity = 0
                    let amountOfActivities = 0

                    //Calculate the total score from activity
                    canvasDecaying.map((l) =>
                        l.layerActivities
                            ?.filter((la) => la.activity === activity.id)
                            .map(
                                (la) =>
                                    (totalScoreActivity +=
                                        la.decayingAverage &&
                                        amountOfActivities++)
                            )
                    )

                    //Calculate the total score from activity inside this architecture layer
                    canvasDecaying
                        .filter((l) => l.architectureLayer === layer.id)
                        .map((l) =>
                            l.layerActivities
                                ?.filter((la) => la.activity === activity.id)
                                .map(
                                    (la) =>
                                        (totalScoreArchitectureActivity +=
                                            la.decayingAverage)
                                )
                        )

                    return {
                        activity: activity.id,
                        decayingAverage:
                            ((totalScoreActivity / amountOfActivities) *
                                totalScoreArchitectureActivity) /
                            totalScoreActivity,
                    } as DecayingAveragePerActivity
                }),
            }
        }) as DecayingAveragePerLayer[]
    }

    private static GetDecayingAverageFromOneOutcomeType(
        results: ProfessionalTaskResult[]
    ): number {
        let totalGradeScore = 0.0

        const recentResult = results.at(0)
        if (recentResult && recentResult.grade) {
            const pastResults = results.slice(1, results.length - 1)
            if (pastResults.length > 0) {
                pastResults.forEach(
                    (r) => (totalGradeScore += r.grade ? r.grade : 0)
                )
                totalGradeScore =
                    (totalGradeScore / pastResults.length) * 0.35 +
                    recentResult.grade * 0.65
            } else {
                totalGradeScore = recentResult.grade
            }
        }

        return totalGradeScore
    }

    private static groupBy<T>(
        arr: T[],
        fn: (item: T) => any
    ): Record<string, T[]> {
        return arr.reduce<Record<string, T[]>>((prev, curr) => {
            const groupKey = fn(curr)
            const group = prev[groupKey] || []
            group.push(curr)
            return { ...prev, [groupKey]: group }
        }, {})
    }
}

export interface DecayingAveragePerActivity {
    outcome: number
    activity: number
    decayingAverage: number
}

export interface DecayingAveragePerLayer {
    architectureLayer: number
    layerActivities: DecayingAveragePerActivity[]
}
