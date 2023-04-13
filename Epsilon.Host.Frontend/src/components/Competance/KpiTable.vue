<template>
    <table v-if="!!data">
        <thead>
        <tr>
            <td/>
            <th v-for="i of hboIDomain.activities">{{ i.name }}</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="(ac , i) of hboIDomain.architectureLayers">
            <th>{{ ac.name }}</th>
            <td v-for="(ar ,x) of hboIDomain.activities" :style="{backgroundColor: getCellColor(i, x)?.color}">
                {{ getKpis(i, x).length }}
            </td>
        </tr>
        </tbody>
    </table>
</template>

<script lang="ts">
import {MasteryLevel} from "/@/logic/Api";

export default {
    name: "KpiTable",
    props: {
        hboIDomain: {
            default: {}
        },
        data: {
            default: {}
        }
    },
    methods: {
        getKpis(arId: string, acId: string): [] {
            return this.data?.filter(o => parseInt(o.architectureLayerId) === parseInt(arId) && parseInt(o.activityId) === parseInt(acId))
        },
        getCellColor(arId: number, acId: number) {
            const maxLevelId =  Math.max(this.getKpis(arId, acId).map(i => {
                return i.masteryLevelId;
            }))
            const maxLevels = this.hboIDomain.masteryLevels as MasteryLevel[]
            console.log(maxLevels[maxLevelId - 1], maxLevels)
            return maxLevels[maxLevelId - 1]
        }
    }
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
