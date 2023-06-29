<template>
    <div v-if="data" class="performance-dashboard">
        <CompetenceProfileComponent
            :data="filteredProfessionalTaskOutcomes"
            :domain="data.hboIDomain" />
        <CompetenceProfileLegend :domain="data.hboIDomain" />
        <div />
        <CompetenceGraph
            :data="filteredProfessionalTaskOutcomes"
            :domain="data.hboIDomain" />
        <PersonalDevelopmentGraph
            :data="filteredProfessionalSkillOutcomes"
            :domain="data?.hboIDomain"></PersonalDevelopmentGraph>
    </div>
    <RoundLoader v-else class="loading-icon" />
</template>

<script lang="ts" setup>
import { Api, CompetenceProfile, HttpResponse } from "../logic/Api"
import CompetenceProfileComponent from "@/components/CompetenceProfile.vue"
import CompetenceProfileLegend from "@/components/CompetenceProfileLegend.vue"
import CompetenceGraph from "@/components/CompetenceGraph.vue"
import { computed, onMounted, Ref, ref } from "vue"
import RoundLoader from "@/components/RoundLoader.vue"
import PersonalDevelopmentGraph from "@/components/PersonalDevelopmentGraph.vue"

const props = defineProps<{
    tillDate: Date | undefined
}>()

const data: Ref<CompetenceProfile | undefined> = ref(undefined)

const App = new Api()

const filteredProfessionalTaskOutcomes = computed(() => {
    if (!data.value?.professionalTaskOutcomes) {
        return []
    }

    if (!props.tillDate) {
        return data.value?.professionalTaskOutcomes
    }

    return data.value.professionalTaskOutcomes.filter(
        (o) => o.assessedAt < props.tillDate
    )
})

const filteredProfessionalSkillOutcomes = computed(() => {
    if (!data.value?.professionalSkillOutcomes) {
        return []
    }

    if (!props.tillDate) {
        return data.value?.professionalSkillOutcomes
    }

    return data.value?.professionalSkillOutcomes?.filter(
        (o) => o.assessedAt < props.tillDate
    )
})

onMounted(() => {
    App.component
        .componentDetail("competence_profile", {
            startDate: "02-26-2023",
            endDate: "05-26-2023",
        })
        .then((r: HttpResponse<CompetenceProfile>) => {
            data.value = r.data
        })
})
</script>

<style scoped>
.performance-dashboard {
    grid-template-columns: 1fr;
}

.loading-icon {
    width: 64px;
    height: 64px;
}

.loading-icon {
    width: 64px;
    height: 64px;
}

@media screen and (min-width: 580px) {
    .performance-dashboard {
        display: grid;
        grid-template-columns: 1fr 5fr 1fr;
        gap: 2rem 0;
        align-items: center;
        justify-items: center;
    }
}
</style>
