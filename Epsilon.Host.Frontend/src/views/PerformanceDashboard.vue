<template>
  <KpiMatrix />
  <KpiTable />
</template>

<script>
import KpiMatrix from "@/components/KpiMatrix.vue";
import KpiTable from "@/components/KpiTable.vue";

export default {
    name: "PerformanceDashboard",
    components: {KpiTable, KpiMatrix}
}
</script>

<style scoped>

</style>
