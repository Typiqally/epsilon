import {
    IHboIDomain,
    ProfessionalSkillResult,
    ProfessionalTaskResult,
} from "/@/logic/Api"

export class DecayingAverage {
    static GetDecayingAverageTasks(
        domain: IHboIDomain | undefined,
        taskResults: ProfessionalTaskResult[] | undefined | null
    ): DecayingAveragePerLayer[] {
        return domain?.architectureLayers?.map((l) => {
            return {
                architectureLayer: l.id,
                layerActivities: domain.activities?.map((a) => {
                    let totalScoreActivity = 0
                    let totalScoreArchitectureActivity = 0
                    if (taskResults) {
                        const acOutcomes = taskResults.filter(
                            (t) => t.activity === a.id
                        )
                        acOutcomes?.map((result) => {
                            if (result.grade) {
                                totalScoreActivity += result.grade
                            }
                        })
                        acOutcomes
                            .filter((t) => t.architectureLayer === l.id)
                            .map((result) => {
                                console.log(result.grade)
                                totalScoreArchitectureActivity += result?.grade
                            })
                        console.log(acOutcomes)
                        return {
                            activity: a.id,
                            decayingAverage:
                                ((totalScoreActivity / acOutcomes.length) *
                                    totalScoreArchitectureActivity) /
                                totalScoreActivity,
                        } as DecayingAveragePerActivity
                    }
                }),
            }
        }) as DecayingAveragePerLayer[]
    }

    static GetDecayingAverageSkills(
        domain: IHboIDomain | undefined,
        skillResults: ProfessionalSkillResult[] | undefined | null
    ): DecayingAveragePerSkill[] {
        return domain?.professionalSkills?.map((s) => {
            let decayingAverage = 0
            skillResults
                ?.filter((outcome) => outcome.skill === s.id)
                ?.map((result) => {
                    if (result.grade) {
                        decayingAverage =
                            decayingAverage * 0.35 + result.grade * 0.65
                    }
                })
            return {
                skill: s.id,
                decayingAverage: decayingAverage as number,
            }
        }) as DecayingAveragePerSkill[]
    }
}

export interface DecayingAveragePerActivity {
    activity?: number
    decayingAverage?: number
}

export interface DecayingAveragePerLayer {
    architectureLayer?: number
    layerActivities?: DecayingAveragePerActivity[] | null
}

export interface DecayingAveragePerSkill {
    skill?: number
    decayingAverage?: number
}
