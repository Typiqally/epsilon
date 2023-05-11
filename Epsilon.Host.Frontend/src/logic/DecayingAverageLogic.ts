import {
    IHboIDomain,
    ProfessionalSkillResult,
    ProfessionalTaskResult,
} from "/@/logic/Api"

export class DecayingAverageLogic {
    public static GetAverageSkillOutcomeScores(
        taskResults: ProfessionalSkillResult[],
        domain: IHboIDomain
    ): DecayingAveragePerSkill[] {
        const listOfResults = Object.entries(
            this.groupBy(taskResults, (r) => r.outcomeId)
        ).map(([, j]) => {
            return {
                decayingAverage: this.GetDecayingAverageFromOneOutcomeType(j),
                skill: j.at(0)?.skill,
            }
        })

        return domain.professionalSkills?.map((s) => {
            let score = 0.0
            const filteredResults = listOfResults.filter(
                (r) => r.skill === s.id
            )
            filteredResults.map((result) => {
                score += result.decayingAverage
            })
            return {
                decayingAverage: score / filteredResults.length,
                skill: s.id,
            } as DecayingAveragePerSkill
        }) as DecayingAveragePerSkill[]
    }

    public static GetAverageTaskOutcomeScores(
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

    /**
     * Explanation of process can be found here: https://community.canvaslms.com/t5/Canvas-Basics-Guide/What-are-Outcomes/ta-p/75#decaying_average
     * @param results
     * @constructor
     * @private
     */
    private static GetDecayingAverageFromOneOutcomeType(
        results: ProfessionalTaskResult[] | ProfessionalSkillResult[]
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
                    parseFloat(
                        (totalGradeScore / pastResults.length).toFixed(2)
                    ) *
                        0.35 +
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

export interface DecayingAveragePerSkill {
    skill: number
    decayingAverage: number
}
