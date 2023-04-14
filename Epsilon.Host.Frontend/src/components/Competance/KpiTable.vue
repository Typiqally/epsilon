<template>
  <table v-if="!!props.data">
    <thead>
      <tr>
        <td />
        <th v-for="activity of props.domain.activities">
          {{ activity.name }}
        </th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="(architectureLayer , i) of props.domain.architectureLayers">
        <th>{{ architectureLayer.name }}</th>
        <td
          v-for="(_, x) of props.domain.activities"
          :style="{backgroundColor: getCellColor(i, x)?.color}"
        >
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
    return props.data.filter(o => o.architectureLayer === parseInt(arId) && o.activity === parseInt(acId))
}

function getCellColor(arId: string, acId: string): MasteryLevel | undefined {
    const kpis = getKpis(arId, acId).sort((a, b) => {
        return props.domain.masteryLevels?.find(masteryLevel => masteryLevel.id == b?.masteryLevel).level - props.domain.masteryLevels?.find(ml => ml.id == a?.masteryLevel).level
    })

    return props.domain.masteryLevels.find(masteryLevel => masteryLevel.id == kpis[0]?.masteryLevel)
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
