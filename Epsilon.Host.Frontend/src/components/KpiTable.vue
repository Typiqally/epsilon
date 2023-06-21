<script lang="ts" setup>
import { Api } from "../logic/Api"
import { Ref, ref } from "vue"

const api = new Api()
const today = new Date()
const otherMonth = new Date()
otherMonth.setMonth(otherMonth.getMonth() - 3)

const data: Ref<unknown> = ref<unknown>()
const Kpis: Ref<unknown> = ref<unknown>([])

api.component
    .componentDetail("kpi_table", {
        startDate: formatDate(otherMonth),
        endDate: formatDate(today),
    })
    .then((result) => {
        data.value = result.data
        Kpis.value = result.data.entries
    })

function formatDate(date: Date): string {
    const year = date.getFullYear()
    const month = `${date.getMonth() + 1}`.padStart(2, "0")
    const day = date.getDate()
    return `${year}-${month}-${day}`
}
</script>

<template>
    <table v-if="data">
        <tr>
            <th>KPI</th>
            <th>Assignments</th>
            <th>Grades</th>
        </tr>
        <tr v-for="KPI of Kpis" :key="KPI">
            <td :style="{
                border: '3px solid #' + KPI.level.color,
            }"> 
                {{ KPI.kpi }}
            </td>
            <td>
                <div
                    v-for="assignment of KPI.assignments"
                    :style="{ textAlign: 'start' }"
                >
                    <a :href="assignment.link">{{ assignment.name }}</a>
                </div>
            </td>
            <td>
                <div v-for="assignment of KPI.assignments">
                    {{ assignment.grade }}
                </div>
            </td>
        </tr>
    </table>
</template>

<style scoped>
td,
th {
    border: 2px solid;
    padding: 1rem;
}
</style>
