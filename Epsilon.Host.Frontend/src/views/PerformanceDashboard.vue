<template>
    <div v-if="data">
        <KpiMatrix :domain="data.hboIDomain"></KpiMatrix>
        <KpiTable :domain="data.hboIDomain" :data="data.professionalTaskOutcomes"></KpiTable>
        <PersonalDevelopmentMatrix :domain="data.hboIDomain" :data="data.professionalSkillOutcomes"></PersonalDevelopmentMatrix>
    </div>
</template>

<script lang="ts" setup>
import {Api, HttpResponse, CompetenceProfile} from "@/logic/Api";
import KpiMatrix from "@/components/Competance/KpiMatrix.vue";
import KpiTable from "@/components/Competance/KpiTable.vue";
import PersonalDevelopmentMatrix from "@/components/Competance/PersonalDevelopmentMatrix.vue";
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

</style>
