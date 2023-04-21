import {
    IHboIDomain,
    ProfessionalSkillResult,
    ProfessionalTaskResult,
} from "/@/logic/Api"

export class DecayingAverage {
    static GetDecayingAverageTasks(
        domain: IHboIDomain | undefined,
        taskResults: ProfessionalTaskResult[] | undefined
    ): DecayingAveragePerLayer[] {
        return domain?.architectureLayers?.map((l) => {
            return {
                architectureLayer: l.id,
                layerActivities: domain.activities?.map((a) => {
                    let decayingAverage = 0
                    taskResults
                        ?.filter(
                            (t) =>
                                t.architectureLayer === l.id &&
                                t.activity === a.id
                        )
                        ?.map((result) => {
                            if (result.grade) {
                                decayingAverage =
                                    decayingAverage * 0.35 + result.grade * 0.65
                            }
                        })

                    return {
                        activity: a.id,
                        decayingAverage: decayingAverage,
                    } as DecayingAveragePerActivity
                }),
            }
        }) as DecayingAveragePerLayer[]
    }

    static GetDecayingAverageSkills(
        domain: IHboIDomain,
        skillResults: ProfessionalSkillResult[]
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
