<template>
    <div class="banner">
        <img
            src="../assets/Epsilon_Logo_Blue-Blue.png"
            alt="logo"
            class="logo" />
        <div class="selection-boxes">
            <div class="profileselect dropdown">Profile</div>
            <select name="term" id="term" class="termselect dropdown">
                <option
                    v-for="term of terms"
                    :key="term.name"
                    :value="term.name">
                    {{ term.name }}
                </option>
            </select>
        </div>
    </div>
    <PerformanceDashboard />
</template>

<script lang="ts" setup>
import { EnrollmentTerm } from "../logic/Api"
import PerformanceDashboard from "./PerformanceDashboard.vue"

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
</style>
