<script lang="ts" setup>
import { Api } from "../logic/Api"
import { Ref, ref } from "vue"

const api = new Api()
const today = new Date()
const otherMonth = new Date()
otherMonth.setMonth(otherMonth.getMonth() - 3)
const data: Ref<unknown | undefined> = ref(undefined)
const Kpis: Ref<unknown | undefined> = ref(undefined)
api.component
    .componentDetail("kpi_matrix", {
        startDate:
            otherMonth.getFullYear() +
            "-" +
            `${otherMonth.getMonth() + 1}`.padStart(2, "0") +
            "-" +
            otherMonth.getDate(),
        endDate:
            today.getFullYear() +
            "-" +
            `${today.getMonth() + 1}`.padStart(2, "0") +
            "-" +
            today.getDate(),
    })
    .then(async (result) => (data.value = result.data))
    .then(() => {
        Kpis.value = {}
        data.value.kpiMatrixAssignments.map((assignment) => {
            assignment.outcomes.map(function(outcome) {
                Kpis.value[outcome.id as string] = outcome
            })
        })
    })
</script>

<template>
  <table v-if="!!data">
    <tr>
      <th />
      <th
        v-for="assignemnt of data.kpiMatrixAssignments"
        :key="assignemnt"
      >
        {{ assignemnt.name }}
      </th>
    </tr>
    <tr
      v-for="KPI of Kpis"
      :key="KPI"
    >
      <th>{{ KPI.title }}</th>
      <td
        v-for="assignemnt of data.kpiMatrixAssignments"
        :key="assignemnt"
        :style="{
          'background-color':
            '#' +
            assignemnt.outcomes.find((o) => o.id === KPI.id)
              ?.gradeStatus.color,
        }"
      >
        {{ assignemnt.outcomes.find((o) => o.id === KPI.id)?.color }}
      </td>
    </tr>
  </table>
</template>

<style scoped></style>
