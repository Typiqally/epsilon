<template>
    <table v-if="!!props.data">
        <thead>
        <tr>
            <td/>
            <th v-for="i of props.domain.activities">{{ i.name }}</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="(ac , i) of props.domain.architectureLayers">
            <th>{{ ac.name }}</th>
            <td v-for="(ar ,x) of props.domain.activities" :style="{backgroundColor: getCellColor(i, x)?.color}">
                {{ getKpis(i, x).length }}
            </td>
        </tr>
        </tbody>
    </table>
</template>

<script lang="ts" setup>
import {HboIDomain, MasteryLevel, ProfessionalTaskOutcome} from "@/logic/Api";

const props = defineProps<{
    domain: HboIDomain
    data: ProfessionalTaskOutcome[]
}>()


function getKpis(arId: string, acId: string): ProfessionalTaskOutcome[] {
    return props.data.filter(o => o.architectureLayerId === parseInt(arId) && o.activityId === parseInt(acId))
}

function getCellColor(arId: string, acId: string): MasteryLevel | undefined {
    const kpis = getKpis(arId, acId).sort((a, b) => {
        return props.domain.masteryLevels?.[a?.masteryLevelId].level - props.domain.masteryLevels?.[b?.masteryLevelId].level
    })
    return props.domain.masteryLevels?.[kpis[0]?.masteryLevelId as number]
}
</script>

<style>
tbody tr th {
    text-align: left;
}

tbody tr td {
    padding: 8px;
    min-width: 100px;
}

tbody tr {
    border-bottom: 2px solid black;
}

tbody td {
    border: 2px solid black;
}

table {
    border-collapse: collapse;
}

thead th {

}
</style>
