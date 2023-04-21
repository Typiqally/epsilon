<template>
    <table v-if="!!props.data" class="competence-profile">
        <thead>
            <tr>
                <td />
                <th
                    v-for="activity of props.domain.activities as Activity[]"
                    :key="activity.id"
                    class="competence-profile-header competence-profile-header-col">
                    {{ activity.name }}
                </th>
            </tr>
        </thead>
        <tbody>
            <tr
                v-for="(architectureLayer, i) of props.domain.architectureLayers as ArchitectureLayer[]"
                :key="i">
                <th
                    class="competence-profile-header competence-profile-header-row">
                    {{ architectureLayer.name }}
                </th>
                <td
                    v-for="(activity, j) of props.domain.activities"
                    :key="j"
                    :style="{ backgroundColor: getCellColor(i, j)?.color }"
                    class="competence-profile-data">
                    {{ getKpis(i, j).length }}
                </td>
            </tr>
        </tbody>
    </table>
</template>

<script lang="ts" setup>
import {
    Activity,
    ArchitectureLayer,
    IHboIDomain,
    MasteryLevel,
    ProfessionalTaskResult,
} from "../logic/Api"

const props = defineProps<{
    domain: IHboIDomain
    data: ProfessionalTaskResult[]
}>()

function getKpis(arId: string, acId: string): ProfessionalTaskResult[] {
    return props.data.filter(
        (o) =>
            o.architectureLayer === parseInt(arId) &&
            o.activity === parseInt(acId)
    )
}

function getCellColor(arId: string, acId: string): MasteryLevel | undefined {
    if (props.domain.masteryLevels == null) {
        return undefined
    }

    const kpis = getKpis(arId, acId).sort((a, b) => {
        return (
            props.domain.masteryLevels?.find(
                (masteryLevel) => masteryLevel.id == b?.masteryLevel
            ).level -
            props.domain.masteryLevels?.find((ml) => ml.id == a?.masteryLevel)
                .level
        )
    })

    return props.domain.masteryLevels.find(
        (masteryLevel) => masteryLevel.id == kpis[0]?.masteryLevel
    )
}
</script>

<style>
.competence-profile {
    border-collapse: collapse;
}

.competence-profile-header {
    padding: 0.5rem;
    font-weight: 600;
}

.competence-profile-header-row {
    text-align: left;
    border: 1px solid #e6e6e6;
    border-left: transparent;
}

.competence-profile-header-col {
    text-align: center;
    border: 1px solid #e6e6e6;
    border-top: transparent;
}

.competence-profile-data {
    padding: 0.5rem;
    min-width: 5rem;
    border: 0 solid #e6e6e6;
}
</style>
