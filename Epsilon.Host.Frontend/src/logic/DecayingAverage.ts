import {
    DecayingAveragePerActivity,
    DecayingAveragePerLayer,
    DecayingAveragePerSkill,
    IHboIDomain,
    ProfessionalSkillResult,
    ProfessionalTaskResult
} from "/@/logic/Api";

export class DecayingAverage {
    static GetDecayingAverageTasks(domain: IHboIDomain|undefined, taskResults: ProfessionalTaskResult[]|undefined): DecayingAveragePerLayer[] {
        return domain?.architectureLayers?.map((l) => {
            return {
                architectureLayer: l.id,
                layerActivities: domain.activities?.map((a) => {
                    let decayingAverage = 0
                    taskResults?.filter(t => t.architectureLayer === l.id && t.activity === a.id)
                        .map((result) => {
                            if (result.grade) {
                                decayingAverage += 0 * .35 + result.grade * .65
                            }
                        })

                    return {
                        activity: a.id,
                        decayingAverage: decayingAverage
                    } as DecayingAveragePerActivity
                })
            }
        }) as DecayingAveragePerLayer[]
    }


    // static GetDecayingAverageSkills(domain: IHboIDomain, skillResults: ProfessionalSkillResult[]): DecayingAveragePerSkill[] {
    //
    // }
}