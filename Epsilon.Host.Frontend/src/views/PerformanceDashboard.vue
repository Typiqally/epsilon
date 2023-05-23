<template>
    <KPIMatrix />
</template>

<script lang="ts" setup>
import { onMounted, ref } from "vue";
import KPIMatrix from "../components/Competance/KPIMatrix.vue";

onMounted(() => {
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
});
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
