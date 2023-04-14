<template>
  <div class="performance-dashboard" v-if="data">
    <CompetenceProfileComponent :domain="data.hboIDomain" :data="data.professionalTaskOutcomes"></CompetenceProfileComponent>
    <CompetenceProfileLegend/>
    <CompetenceGraph :domain="data.hboIDomain"></CompetenceGraph>
    <PersonalDevelopmentMatrix :domain="data.hboIDomain" :data="data.professionalSkillOutcomes"
    ></PersonalDevelopmentMatrix>
  </div>
</template>

<script lang="ts" setup>
import {Api, HttpResponse, CompetenceProfile} from "@/logic/Api";
import CompetenceProfileComponent from "@/components/CompetenceProfile.vue";
import CompetenceProfileLegend from "@/components/CompetenceProfileLegend.vue";
import CompetenceGraph from "@/components/CompetenceGraph.vue";
import PersonalDevelopmentMatrix from "@/components/PersonalDevelopmentGraph.vue";
import {onMounted, ref} from "vue";

const data = ref(undefined);
const App = new Api();

onMounted(() => {
    App.component.competenceProfileList()
        .then((r: HttpResponse<any>) => {
            data.value = r.data as CompetenceProfile
        })
})
</script>

<style scoped>
.performance-dashboard {
    grid-template-columns: 1fr;
}

@media screen and (min-width: 580px) {
    .performance-dashboard {
        display: grid;
        grid-template-columns: 5fr 1fr;
        gap: 4rem 0;
        align-items: center;
    }
}
</style>
