<template>
    <div class="performance-dashboard" v-if="data">
      <KpiTable :domain="data.hboIDomain" :data="data.professionalTaskOutcomes"></KpiTable>
      <KpiLegend />
      <KpiMatrix :domain="data.hboIDomain"></KpiMatrix>
        <PersonalDevelopmentMatrix :domain="data.hboIDomain" :data="data.professionalSkillOutcomes"></PersonalDevelopmentMatrix>
    </div>
</template>

<script lang="ts" setup>
import {Api, HttpResponse, CompetenceProfile} from "@/logic/Api";
import KpiMatrix from "@/components/Competance/KpiMatrix.vue";
import KpiTable from "@/components/Competance/KpiTable.vue";
import PersonalDevelopmentMatrix from "@/components/Competance/PersonalDevelopmentMatrix.vue";
import KpiLegend from "@/components/KpiLegend/KpiLegend.vue";
import {onMounted, ref} from "vue";
const data= ref(undefined);
const App = new Api();

onMounted(() => {
    App.component.competenceProfileList()
        .then((r:HttpResponse<any>) => {
            data.value = r.data as CompetenceProfile
        })

    //App.component.competenceProfileList()
    // .then(response => {
    //     if (response.ok) {
    //         return response.json()
    //     }
    //
    //     return Promise.reject(response);
    // })
    // .then(data => {
    //     console.log(data)
    // })
    // .catch(error => {
    //     if (error.status == 401) {
    //         router.push('/auth')
    //     }
    // })
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
      gap: 0 2rem;
      align-items: center;
    }
  }
</style>
