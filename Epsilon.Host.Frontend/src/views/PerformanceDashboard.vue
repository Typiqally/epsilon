<template>
    <div v-if="data">
        <KpiMatrix :hbo-i-domain="data.hboIDomain"></KpiMatrix>
        <KpiTable :hbo-i-domain="data.hboIDomain" :data="data.professionalTaskOutcomes"></KpiTable>
        <PersonalDevelopmentMatrix :hbo-i-domain="data.hboIDomain"></PersonalDevelopmentMatrix>
    </div>
</template>

<script lang="ts">
import {Api} from "@/logic/Api";
import KpiMatrix from "@/components/Competance/KpiMatrix.vue";
import KpiTable from "@/components/Competance/KpiTable.vue";
import PersonalDevelopmentMatrix from "@/components/Competance/PersonalDevelopmentMatrix.vue";
import {CompetenceProfile} from "@/logic/Api";
import {HttpResponse} from "@/logic/Api";
let data = {};

export default {
    name: "PerformanceDashboard",
    components: {PersonalDevelopmentMatrix, KpiTable, KpiMatrix},
    data() {
      return {
          App: new Api(),
          data: {}
      }
    },
    mounted() {
        this.App.component.competenceProfileMockList()
            .then((r:HttpResponse<any>) => {
                this.data = r.data as CompetenceProfile
            })

        this.App.component.competenceProfileList()
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
    }
}
</script>

<style scoped>

</style>
