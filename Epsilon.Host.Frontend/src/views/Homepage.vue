<template>
    <div v-if="data" class="wrapper">
        <div class="banner">
            <img
                src="../assets/Epsilon_Logo_Blue-Blue.png"
                alt="logo"
                class="logo" />
            <div class="selection-boxes">
                <DropdownBtn />
            </div>
        </div>
        <ViewTabs />
        <PerformanceDashboard />
    </div>
    <RoundLoader v-else />
</template>

<script lang="ts" setup>
import { Api, CompetenceProfile, HttpResponse } from "../logic/Api"

import PerformanceDashboard from "./PerformanceDashboard.vue"
import RoundLoader from "@/components/RoundLoader.vue"
import ViewTabs from "@/components/ViewTabs.vue"
import DropdownBtn from "@/components/DropdownBtn.vue"
import { onMounted, Ref, ref } from "vue"

const data: Ref<CompetenceProfile | undefined> = ref(undefined)
const App = new Api()

onMounted(() => {
    App.component
        .componentDetail("competence_profile", {
            startDate: "02-26-2023",
            endDate: "05-26-2023",
        })
        .then((r: HttpResponse<CompetenceProfile>) => {
            data.value = r.data
            console.log(r.data.terms)
        })
})
</script>

<style scoped>
.banner {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem 2rem;

    background-color: #f2f3f8;
    height: 9rem;
    width: 100%;
    border-radius: 0.5rem;
}
.logo {
    height: 5rem;
    margin-left: 2rem;
}

.selection-boxes {
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
}

.dropdown:focus,
.dropdown:active {
    outline: transparent;
}
</style>
