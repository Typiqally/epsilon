<template>
  <table
    v-if="!!props.data"
    class="kpitable"
  >
    <thead>
      <tr>
        <td />
        <th
          v-for="activity of props.domain.activities"
          class="kpitable__header kpitable__header-column"
        >
          {{ activity.name }}
        </th>
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="(architectureLayer, i) of props.domain.architectureLayers"
        :key="i"
      >
        <th class="kpitable__header kpitable__header-row">
          {{ architectureLayer.name }}
        </th>
        <td
          v-for="(activity, j) of props.domain.activities"
          :key="j"
          :style="{backgroundColor: getCellColor(i, j)?.color}"
          class="kpitable__data"
        >
          {{ getKpis(i, j).length }}
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
.kpitable {
    border-collapse: collapse;
    height: 250px;
}

.kpitable__header {
    padding: 0.5rem;
    font-weight: 600;
}

.kpitable__header-row {
    text-align: left;
    border: 1px solid #e6e6e6;
    border-left: transparent;
}

.kpitable__header-column {
    text-align: center;
    border: 1px solid #e6e6e6;
    border-top: transparent;
}

.kpitable__data {
    padding: 0.5rem;
    min-width: 5rem;
    border: 0px solid #e6e6e6;
}
</style>
