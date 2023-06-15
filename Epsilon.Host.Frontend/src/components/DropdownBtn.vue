<template>
    <div class="profileselect dropdown">Profile</div>
    <select id="term" name="term" class="termselect dropdown">
        <option v-for="term of terms" :key="term.name" :value="term.name">
            {{ term.name }}
        </option>
    </select>
</template>

<script lang="ts" setup>
import { EnrollmentTerm } from "../logic/Api"

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

.profileselect {
    margin-right: 4rem;
}

.dropdown {
    background-color: #fff;
    padding: 1rem 2rem;
    border: 5px;

    font-family: Inter, system-ui, Avenir, Helvetica, Arial, sans-serif;
    font-size: 1rem;
}

.dropdown:active, 
.dropdown:focus {
    outline: none;
    border: none;
}

.slide-item {
    display: flex;
    flex-direction: column;
    position: absolute;
    background-color: white;
    padding: 1rem;
    margin-top: 3.5rem;
    border-radius: 6px;
    border: 1px solid black;
}

</style>
