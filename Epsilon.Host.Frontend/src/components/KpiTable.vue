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
    .componentDetail("kpi_table", {
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
        Kpis.value = []
        data.value.entries.map((entry, index) => {
            Kpis.value[index] = entry
        })
    })
    .then(() => {
        console.log("kpi.value:", Kpis.value)
        console.log("data:", data)
    })
</script>

<template>
    <table v-if="!!data">
        <tr>
            <th>
                KPI
            </th>
            <th style="">
                Assignments
            </th>
            <th>
                Grades
            </th>
        </tr>
        <tr v-for="KPI of Kpis" :key="KPI">
            <td>{{ KPI.kpi }}</td>
            <td>
                <div v-for="assignment of KPI.assignments">
                    <a :href="`${assignment.link}`">{{ assignment.name }}</a>
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
    table {
        text-align: start;
    }
    th, td {
        padding: 1rem;
    }

</style>

<!-- 
:key="entry"   
:style="{
    'background-color':
        '#' +
        entry.outcomes.find((o) => o.id === KPI.id)
            ?.gradeStatus.color,
}" -->
