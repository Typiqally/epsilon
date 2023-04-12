<template>
  <KpiMatrix/>
  <KpiTable/>
</template>

<script setup lang="ts">
import {onMounted} from "vue";

import KpiMatrix from "@/components/KpiMatrix.vue";
import KpiTable from "@/components/KpiTable.vue";
import router from "@/router";

onMounted(() => {
    fetch('https://localhost:7084/component/competence_profile?courseId=1', {credentials: "include"})
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
