<script lang="ts" setup>
import { Api } from "../../logic/Api";
import { Ref, ref } from "vue";

const api = new Api();
const today = new Date();
const OtherMonth = new Date();
OtherMonth.setMonth(OtherMonth.getMonth() - 3);
const result = today.toLocaleDateString("en-US", {
    // you can use undefined as first argument
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
});
const data: Ref<any | undefined> = ref(undefined);
const Kpis: Ref<any | undefined> = ref(undefined);
api.component
    .componentDetail("kpi_matrix", {
        startDate:
            OtherMonth.getFullYear() +
            "-" +
            `${OtherMonth.getMonth() + 1}`.padStart(2, "0") +
            "-" +
            OtherMonth.getDate(),
        endDate:
            today.getFullYear() +
            "-" +
            `${today.getMonth() + 1}`.padStart(2, "0") +
            "-" +
            today.getDate(),
    })
    .then(async (result) => (data.value = result.data))
    .then(() => {
        Kpis.value = {};
        data.value.kpiMatrixAssignments.map((assignment) => {
            assignment.outcomes.map(function (outcome) {
                Kpis.value[outcome.id as string] = outcome;
            });
        });
    });
</script>

<template>
    <table v-if="!!data">
        <tr>
            <th />
            <th v-for="assignemnt of data.kpiMatrixAssignments">
                {{ assignemnt.name }}
            </th>
        </tr>
        <tr v-for="KPI of Kpis">
            <th>{{ KPI.title }}</th>
            <td
                v-for="assignemnt of data.kpiMatrixAssignments"
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
