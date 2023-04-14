<template>
  <div class="performance-dashboard" v-if="data">
    <KpiTable :domain="data.hboIDomain" :data="data.professionalTaskOutcomes"></KpiTable>
    <KpiLegend/>
    <KpiMatrix :domain="data.hboIDomain"></KpiMatrix>
    <PersonalDevelopmentMatrix :domain="data.hboIDomain" :data="data.professionalSkillOutcomes"
    ></PersonalDevelopmentMatrix>
  </div>
</template>

<script lang="ts" setup>
import {Api, HttpResponse, CompetenceProfile} from "@/logic/Api";
import KpiMatrix from "@/components/KpiMatrix.vue";
import KpiTable from "@/components/KpiTable.vue";
import PersonalDevelopmentMatrix from "@/components/PersonalDevelopmentMatrix.vue";
import KpiLegend from "@/components/KpiLegend.vue";
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
