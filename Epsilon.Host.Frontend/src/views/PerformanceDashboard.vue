<template>
  <KPIMatrix />
</template>

<script lang="ts" setup>
import KPIMatrix from "../components/Competance/KPIMatrix.vue"
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
