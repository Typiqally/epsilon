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
                    <div
                        class="profile-header-color"
                        :style="{
                            backgroundColor: architectureLayer.color,
                        }"></div>
                    {{ architectureLayer.name }}
                </th>
                <CompetenceProfileCell
                    v-for="(activity, j) of props.domain.activities"
                    :key="j"
                    :name="`${architectureLayer.name} ${activity.name}`"
                    :kpis="getKpis(i, j)"
                    :levels="props.domain.masteryLevels!" />
            </tr>
        </tbody>
    </table>
</template>

<script lang="ts" setup>
import {
    Activity,
    ArchitectureLayer,
    IHboIDomain,
    ProfessionalTaskResult,
} from "../logic/Api"

import CompetenceProfileCell from "./CompetenceProfileCell.vue"

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
</script>

<style>
.competence-profile {
    border-collapse: collapse;
    width: 750px;
}

.competence-profile-header {
    padding: 0.5rem;
    font-weight: 400;
    font-size: 0.9rem;
}

.competence-profile-header-row {
    text-align: left;
    border: 1px solid #e6e6e6;
    border-left: transparent;
    padding-right: 4rem;
    display: flex;
}

.competence-profile-header-col {
    text-align: center;
    border: 1px solid #e6e6e6;
    border-top: transparent;
    width: 6rem;
}

.profile-header-color {
    margin: 3px 10px 0;
    width: 15px;
    height: 15px;
}

.competence-profile-data {
    font-size: 0.9rem;
    padding: 0.5rem;
    min-width: 5rem;
    border: 1px solid #e6e6e6;
    width: 750px;
    font-weight: 400;
    font-size: 0.9rem;
    padding-right: 4rem;
    display: flex;
    width: 6rem;
}

.profile-header-color {
    margin: 3px 10px 0;
    width: 15px;
    height: 15px;
    font-size: 0.9rem;
    border: 1px solid #e6e6e6;
}
</style>
