import {
    IHboIDomain,
    ProfessionalSkillResult,
    ProfessionalTaskResult,
} from "/@/logic/Api"

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
    masteryLevel: number
}

export class DecayingAverageLogic {
    /**
     * Calculate the averages for each skill type
     * @param taskResults
     * @param domain
     * @constructor
     */
    public static GetAverageSkillOutcomeScores(
        taskResults: ProfessionalSkillResult[],
        domain: IHboIDomain
    ): DecayingAveragePerSkill[] {
        const listOfResults = Object.entries(
            this.groupBy(taskResults, (r) => r.outcomeId as unknown as string)
        ).map(([, j]) => {
            return {
                decayingAverage: this.GetDecayingAverageFromOneOutcomeType(j),
                skill: j.at(0)?.skill,
                masteryLevel: j
                    .sort((a) => a.masteryLevel as never as number)
                    .at(0)?.masteryLevel,
            } as DecayingAveragePerSkill
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
                masteryLevel: filteredResults
                    .sort((a) => a.masteryLevel as never as number)
                    .at(0)?.masteryLevel,
            } as DecayingAveragePerSkill
        }) as DecayingAveragePerSkill[]
    }

    /**
     * Calculate the averages for each task type divided in architecture layers.
     * @param taskResults
     * @param domain
     * @constructor
     */
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

    /**
     * Calculate average of given tasks divided in architectural layers
     * @param taskResults
     * @param domain
     * @constructor
     * @private
     */
    private static GetDecayingAverageForAllOutcomes(
        taskResults: ProfessionalTaskResult[],
        domain: IHboIDomain
    ): DecayingAveragePerLayer[] {
        return domain.architectureLayers?.map((l) => {
            return {
                architectureLayer: l.id,
                layerActivities: Object.entries(
                    this.groupBy(
                        //Ensure that given results are only relined on the architecture that is currently being used.
                        taskResults.filter(
                            (layer) => layer.architectureLayer === l.id
                        ),
                        (r) => r.outcomeId as unknown as string
                    )
                ).map(([i, j]) => {
                    //From all selected outcomes calculate the decaying average, Give outcome id and activity layer.
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
     * Calculate decaying average described by Canvas: https://community.canvaslms.com/t5/Canvas-Basics-Guide/What-are-Outcomes/ta-p/75#decaying_average
     * !IMPORTANT, The list of results will always have to be a list of the same outcome id. Not a list of equal activity and architecture layer.
     * @param results
     * @constructor
     * @private
     */
    private static GetDecayingAverageFromOneOutcomeType(
        results: ProfessionalTaskResult[] | ProfessionalSkillResult[]
    ): number {
        let totalGradeScore = 0.0

        const recentResult = results.reverse().pop()
        if (recentResult && recentResult.grade) {
            if (results.length > 0) {
                results.forEach(
                    (r) => (totalGradeScore += r.grade ? r.grade : 0)
                )
                totalGradeScore =
                    (totalGradeScore / results.length) * 0.35 +
                    recentResult.grade * 0.65
            } else {
                totalGradeScore = recentResult.grade
            }
        }

        return totalGradeScore
    }

    private static groupBy<T>(
        arr: T[],
        fn: (item: T) => number | string
    ): Record<string, T[]> {
        return arr.reduce<Record<string, T[]>>((prev, curr) => {
            const groupKey = fn(curr)
            const group = prev[groupKey] || []
            group.push(curr)
            return { ...prev, [groupKey]: group }
        }, {})
    }
}
