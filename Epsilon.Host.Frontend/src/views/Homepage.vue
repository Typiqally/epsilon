<template>
    <!-- <div v-if="data" class="wrapper"> -->
    <div class="banner">
        <img
            src="../assets/Epsilon_Logo_Blue-Blue.png"
            alt="logo"
            class="logo" />
        <div class="selection-boxes">
            <div class="profileselect dropdown">Profile</div>
            <select id="term" name="term" class="termselect dropdown">
                <option
                    v-for="term of terms"
                    :key="term.name"
                    :value="term.name">
                    {{ term.name }}
                </option>
            </select>
        </div>
    </div>
    <div class="slider">
        <div class="slider-select"></div>
        <button type="button" class="slider-text">Performance dashboard</button>
        <button type="button" class="slider-text">Competence document</button>
    </div>
    <PerformanceDashboard />
    <!-- </div> -->
    <!-- <RoundLoader v-else /> -->
</template>

<script lang="ts" setup>
import {
    Api,
    CompetenceProfile,
    EnrollmentTerm,
    HttpResponse,
} from "../logic/Api"

import PerformanceDashboard from "./PerformanceDashboard.vue"
import RoundLoader from "@/components/RoundLoader.vue"
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

const terms: Array<EnrollmentTerm> = [
    {
        name: "2223vj",
        start_at: "2023-01-15T23:00:00Z",
        end_at: "2023-08-26T22:00:00Z",
    },
    {
        name: "2223nj",
        start_at: "2022-07-31T22:00:00Z",
        end_at: "2023-03-26T22:00:00Z",
    },
    {
        name: "2122vj",
        start_at: "2022-01-15T23:00:00Z",
        end_at: "2022-08-26T22:00:00Z",
    },
    {
        name: "2122nj",
        start_at: "2021-08-01T22:00:00Z",
        end_at: "2022-03-26T22:00:00Z",
    },
]
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

.termselect {
    margin-left: 2rem;
}

.dropdown {
    background-color: #fff;
    padding: 1rem 2rem;
    border: 5px;

    font-family: Inter, system-ui, Avenir, Helvetica, Arial, sans-serif;
    font-size: 1rem;
}

.dropdown:focus,
.dropdown:active {
    outline: transparent;
}

.slider {
    background-color: #f2f3f8;
    display: flex;
    flex-direction: row;
    align-items: center;
    margin-top: 3rem;
    padding: 5px 10px;
    width: fit-content;
    height: 2.5rem;
    gap: 2rem;
    border-radius: 10px;
}

.slider-select {
    background-color: #fff;
    width: 13rem;
    height: 2.25rem;
    position: absolute;
    z-index: 1;
    border-radius: 7px;
}

.slider-text {
    position: relative;
    z-index: 2;
    padding: 0 1rem;
    background-color: transparent;
    border: none;
}

.slider-text:hover {
    border: none;
}

.slider-text:focus,
.slider-text:active {
    outline: transparent;
}
</style>
