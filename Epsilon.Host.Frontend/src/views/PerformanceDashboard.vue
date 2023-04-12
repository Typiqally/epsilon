<template>
    <div v-if="data !== {}">
        <KpiMatrix :hbo-i-domain="data.hboIDomain"></KpiMatrix>
        <KpiTable :hbo-i-domain="data.hboIDomain"></KpiTable>
        <PersonalDevelopmentMatrix></PersonalDevelopmentMatrix>
    </div>
</template>

<script setup lang="ts">
import {onMounted} from "vue";
import router from "@/router";
import PersonalDevelopmentMatrix from "@/components/Competance/PersonalDevelopmentMatrix.vue";
import KpiMatrix from "@/components/KpiMatrix.vue";
import KpiTable from "@/components/KpiTable.vue";
import {Api} from "./../Logic/Api";
let data = {};

onMounted(() => {
    const App = new Api()


    App.component.competenceProfileMockList()
        .then((r) => {
            data = r.data
        })

    App.component.competenceProfileList()
        .then(response => {
            if (response.ok) {
                return response.json()
            }

            return Promise.reject(response);
        })
        .then(data => {
            console.log(data)
        })
        .catch(error => {
            if (error.status == 401) {
                router.push('/auth')
            }
        })
})
</script>

<style scoped>

</style>
